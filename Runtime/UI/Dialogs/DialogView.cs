// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace Amax.UI.Dialogs
{
    public class DialogView: MonoBehaviour, IDialogView
    {

        [Header("Rects")] 
        [SerializeField] private RectTransform dialogRoot;
        [SerializeField] private RectTransform dialogBackgroundRoot;
        [SerializeField] private RectTransform dialogWindowRoot;

        [Header("Base")]
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Image image;
        
        [Header("Buttons")]
        [SerializeField] private RectTransform buttonRoot;
        [SerializeField] private GameObject buttonPrefab;
        
        [Header("Content")]
        [SerializeField] private RectTransform contentRoot;
        
        private IDialogViewAnimations DialogViewAnimations { get; set; }
        
        public void Close()
        {
            StartCoroutine(DisappearCoroutine());
        }

        private IEnumerator DisappearCoroutine()
        {
            if (DialogViewAnimations != null)
                yield return DialogViewAnimations.DisappearAnimationCoroutine(dialogRoot, dialogWindowRoot,
                    dialogBackgroundRoot);
            Destroy(gameObject);
        }

        public void Configure(IDialogContent dialogContent)
        {

            DialogViewAnimations ??= GetComponent<IDialogViewAnimations>();

            title.text = dialogContent.Title; // Title
            text.text = dialogContent.Text; //Text
            
            // Image
            if (dialogContent.Image != null)
            {
                image.sprite = dialogContent.Image;
            }
            else
            {
                image.gameObject.SetActive(image.sprite != null);
            }
            
            AddContent(dialogContent); // Content
            
            AddButtons(dialogContent); // Buttons
            
            // Configurators
            foreach (var configurator in GetComponentsInChildren<IDialogViewConfigurator>())
            {
                configurator.Configure(this, dialogContent);
            }

            StartCoroutine(AppearCoroutine());

        }

        private IEnumerator AppearCoroutine()
        {
            if (DialogViewAnimations != null)
                yield return DialogViewAnimations.AppearAnimationCoroutine(dialogRoot, dialogWindowRoot,
                    dialogBackgroundRoot);
        }

        private void AddButtons(IDialogContent content)
        {
            foreach (var buttonContent in content.Buttons)
            {
                var buttonGO = Instantiate(buttonPrefab, buttonRoot);
                var buttonView = buttonGO.GetComponent<IDialogButtonView>();
                buttonView.Apply(content, buttonContent);
                buttonView.OnClick += view =>
                {
                    OnDialogButtonClick?.Invoke(view.ButtonContent.Code);
                };
            }
        }

        private void AddContent(IDialogContent content)
        {
            // Content
            if (content.Content != null)
            {
                (content.Content.transform as RectTransform)?.SetParent(contentRoot);
            }
            // Content prefab
            if (content.ContentPrefab != null)
            {
                Instantiate(content.ContentPrefab, contentRoot);
            }
        }

        public event Action<int> OnDialogButtonClick;
        
    }
}