// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using UnityEngine;

#endregion

namespace Amax.UI.Toasts
{
    public class Toasts: MonoBehaviour
    {
        
        public static IToasts Instance { get; set; }

        private void Awake()
        {
            Instance ??= GetComponent<IToasts>();
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public static IToastDurationProvider ToastDurationProvider => Instance.ToastDurationProvider;
        
        public static IToast ShowToast(IToastContent content)
            => Instance.ShowToast(content);

        public static IToast ShowToast(string title, string text, EToastDuration duration = EToastDuration.Normal,
            Sprite icon = null, EToastStyle style = EToastStyle.Default)
            => Instance.ShowToast(new ToastContent()
            {
                Title = title,
                Text = text,
                Icon = icon,
                Style = style,
                Duration = Instance.ToastDurationProvider.GetDuration(duration)
            });

        public static IToast ShowToast(string title, string text, float duration, Sprite icon = null,
            EToastStyle style = EToastStyle.Default)
            => Instance.ShowToast(new ToastContent()
            {
                Title = title,
                Text = text,
                Icon = icon,
                Style = style,
                Duration = duration
            });
        
    }
}