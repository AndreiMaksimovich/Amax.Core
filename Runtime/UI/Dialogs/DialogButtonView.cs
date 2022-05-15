// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace Amax.UI.Dialogs
{
    public class DialogButtonView: MonoBehaviour, IDialogButtonView
    {

        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private Image icon;
        
        public IDialogButtonContent ButtonContent { get; private set; }
        
        public void Apply(IDialogContent dialogContent, IDialogButtonContent buttonContent)
        {
            ButtonContent = buttonContent;
            label.text = buttonContent.Label;
            icon.sprite = buttonContent.Icon;
            icon.gameObject.SetActive(icon.sprite != null);
            button.onClick.AddListener(() => OnClick?.Invoke(this));
        }

        public event Action<IDialogButtonView> OnClick;
    }
    
}