// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace Amax.UI.Toasts
{
    
    public class ToastView: MonoBehaviour, IToastView
    {

        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Image icon;
        [SerializeField] private RectTransform contentRoot;
        [SerializeField] private RectTransform toastRoot;

        public float DurationLeft => ToastContent.Duration - _lifeTime;
        private float _lifeTime = 0f;
        
        public IToastContent ToastContent { get; private set; }
        private RectTransform ToastContainer { get; set; }
        
        public void Configure(IToastContent content, RectTransform toastContainer)
        {
            ToastContent = content;
            ToastContainer = toastContainer;
            ConfigureUI();
            ApplyConfigurators();
            StartCoroutine(ToastLifeCycleCoroutine());
        }

        private IEnumerator ToastLifeCycleCoroutine()
        {
            var animator = GetComponent<IToastViewAnimator>();
            if (animator != null) yield return animator.AppearAnimationCoroutine(ToastContainer, toastRoot);
            while (_lifeTime < ToastContent.Duration)
            {
                _lifeTime += Time.deltaTime;
                yield return null;
            }
            OnClose?.Invoke(this);
            if (animator != null) yield return animator.DisappearAnimationCoroutine(ToastContainer, toastRoot);
            Destroy(toastRoot.gameObject);
        }

        private void ConfigureUI()
        {
            var content = ToastContent;
            
            // title
            title.text = content.Title;
            title.gameObject.SetActive(!string.IsNullOrEmpty(title.text));
            
            // text
            text.text = content.Text;
            text.gameObject.SetActive(!string.IsNullOrEmpty(text.text));
            
            // icon
            if (content.Icon != null) icon.sprite = content.Icon;
            icon.gameObject.SetActive(icon.sprite != null);
            
            // content - go
            if (content.Content != null)
            {
                content.Content.transform.SetParent(contentRoot);
            }
            
            // content - prefab
            if (content.ContentPrefab != null)
            {
                Instantiate(content.ContentPrefab, contentRoot);
            }

        }

        private void ApplyConfigurators()
        {
            foreach (var configurator in GetComponentsInChildren<IToastViewConfigurator>())
            {
                configurator.Configure(this);
            }
        }

        public event Action<IToastView> OnClose;
        
    }
    
}