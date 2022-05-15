// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;

#endregion

namespace Amax.UI.Toasts
{
    public class Toast: IToast
    {

        private IToastView ToastView { get; }
        
        public Toast(IToastView toastView)
        {
            ToastView = toastView;
            ToastContent = toastView.ToastContent;
            ToastView.OnClose += OnToastViewClose;              
        }

        private void OnToastViewClose(IToastView toastView)
        {
            IsShown = false;
            OnClose?.Invoke(this);
        }

        public bool IsShown { get; private set; } = true;
        public float DurationLeft => ToastView.DurationLeft;
        public event Action<IToast> OnClose;
        public IToastContent ToastContent { get; private set; }
    }
}