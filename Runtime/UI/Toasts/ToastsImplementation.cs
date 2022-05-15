// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using UnityEngine;

#endregion

namespace Amax.UI.Toasts
{
    public class ToastsImplementation: MonoBehaviour, IToasts
    {

        [SerializeField] private RectTransform toastRoot;
        
        private void Awake()
        {
            ToastDurationProvider ??= GetComponent<IToastDurationProvider>();
            ToastPrefabProvider ??= GetComponent<IToastPrefabProvider>();
        }

        public IToast ShowToast(IToastContent content)
        {
            var toastPrefab = ToastPrefabProvider.GetPrefab(content);
            var toastGameObject = Instantiate(toastPrefab, toastRoot);
            var toastView = toastGameObject.GetComponent<IToastView>();
            toastView.Configure(content, toastRoot);
            return new Toast(toastView);
        }

        public IToastDurationProvider ToastDurationProvider { get; set; }
        public IToastPrefabProvider ToastPrefabProvider { get; set; }
        
    }
}