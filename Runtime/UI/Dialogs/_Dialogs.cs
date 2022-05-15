// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Amax.UI.Dialogs
{

    public interface IDialogs
    {
        public IMessageDialog ShowMessageDialog(IDialogContent dialogContent);
        public IResultDialog ShowResultDialog(IDialogContent dialogContent);
        
        public IDialogButtonFactory DialogButtonFactory { get; }
        
    }

    public interface IDialogBase
    {
        bool IsOpened { get; }
        public void Close();
    }
    
    public interface IMessageDialog: IDialogBase
    {
        public event Action<IMessageDialog> OnClose;
    }

    public interface IResultDialog : IDialogBase
    {
        public int ResultCode { get; }
        public event Action<IResultDialog, int> OnClose;
    }

    public interface IDialogContent
    {
        public string Title { get; }
        public string Text { get; }
        public Sprite Image { get; }
        public GameObject Content { get; }
        public GameObject ContentPrefab { get; }
        public GameObject DialogPrefab { get; }
        public EDialogStyle Style { get; }
        public EDialogSize Size { get; }
        public List<IDialogButtonContent> Buttons { get; }
    }
    
    public interface IDialogButtonContent
    {
        public int Code { get; }
        public EDialogButtonState State { get; }
        public string Label { get; }
        public Sprite Icon { get; }
    }

    public enum EDialogButtonType
    {
        Unknown = 0,
        OK = 1,
        Cancel = 2,
        Yes = 3,
        No = 4,
        Abort = 5,
        Close = 6
    }

    public enum EDialogSize
    {
        Small,
        Medium,
        Large
    }

    public enum EDialogButtonState
    {
        Enabled,
        Disabled,
        Hidden
    }

    public enum EDialogStyle
    {
        Default,
        Info,
        Warning,
        Error
    }
    
    public interface IDialogButtonFactory
    {
        public IDialogButtonContent GetButton(EDialogButtonType type);
        public IDialogButtonContent GetButton(string title, int buttonCode, Sprite icon = null, EDialogButtonState state = EDialogButtonState.Enabled);
    }

    public interface IDialogPrefabProvider
    {
        public GameObject GetPrefab(IDialogContent content);
    }

    public interface ICustomDialogPrefabProvider
    {
        public bool IsSupported(IDialogContent content);
        public GameObject GetPrefab(IDialogContent content);
    }

    public interface IDialogView
    {
        public void Close();
        public void Configure(IDialogContent dialogContent);
        public event Action<int> OnDialogButtonClick;
    }

    public interface IDialogButtonView
    {
        public IDialogButtonContent ButtonContent { get; }
        public void Apply(IDialogContent dialogContent, IDialogButtonContent buttonContent);
        public event Action<IDialogButtonView> OnClick;
    }

    public interface IDialogViewAnimations
    {
        public IEnumerator AppearAnimationCoroutine(RectTransform dialogRoot, RectTransform dialogWindow,
            RectTransform background);
        public IEnumerator DisappearAnimationCoroutine(RectTransform dialogRoot, RectTransform dialogWindow,
            RectTransform background);
    }

    public interface IDialogViewConfigurator
    {
        public void Configure(IDialogView dialogView, IDialogContent dialogContent);
    }
   
}