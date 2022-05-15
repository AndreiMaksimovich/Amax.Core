// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using System.Collections;
using UnityEngine;

#endregion

namespace Amax.UI.Toasts
{

    public interface IToasts
    {
        public IToast ShowToast(IToastContent content);
        public IToastDurationProvider ToastDurationProvider { get; }
        public IToastPrefabProvider ToastPrefabProvider { get; }
    }

    public interface IToastContent
    {
        public string Title { get; }
        public string Text { get; }
        public float Duration { get; }
        public Sprite Icon { get; }
        public EToastStyle Style { get; }
        public GameObject Content { get; }
        public GameObject ContentPrefab { get; }
    }

    public interface IToastDurationProvider
    {
        public float GetDuration(EToastDuration duration);
    }

    public interface IToastPrefabProvider
    {
        public GameObject GetPrefab(IToastContent content);
    }

    public interface ICustomToastPrefabProvider
    {
        public bool IsSupported(IToastContent content);
        public GameObject GetPrefab(IToastContent content);
    }

    public enum EToastDuration
    {
        Short,
        Normal,
        Long,
        VeryLong
    }

    public enum EToastStyle
    {
        Default,
        Info,
        Warning,
        Error
    }

    public interface IToast
    {
        public bool IsShown { get; }
        public float DurationLeft { get; }
        public event Action<IToast> OnClose;
        public IToastContent ToastContent { get; }
    }

    public interface IToastView
    {
        public float DurationLeft { get; }
        public IToastContent ToastContent { get; }
        public void Configure(IToastContent content, RectTransform toastContainer);
        public event Action<IToastView> OnClose;
    }

    public interface IToastViewConfigurator
    {
        public void Configure(IToastView toastView);
    }

    public interface IToastViewAnimator
    {
        public IEnumerator AppearAnimationCoroutine(RectTransform toastsRoot, RectTransform toastRoot);
        public IEnumerator DisappearAnimationCoroutine(RectTransform toastsRoot, RectTransform toastRoot);
    }
    
}