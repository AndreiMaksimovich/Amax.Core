// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using Amax.Navigation;
using Amax.UI.Dialogs;
using Amax.UI.Toasts;
using TMPro;
using UnityEngine;

#endregion

namespace Amax.Core.Examples
{

    public class SceneControllerExampleUI : ASceneController
    {

        [Header("Dialog")]
        [SerializeField] private TMP_Dropdown dialogStyleDropdown;
        [SerializeField] private TMP_Dropdown dialogSizeDropdown;

        [Header("Toast")]
        [SerializeField] private TMP_Dropdown toastStyleDropdown;
        [SerializeField] private TMP_Dropdown toastDurationDropdown;

        [Header("Common")] 
        [SerializeField] private TMP_InputField titleInputField;
        [SerializeField] private TMP_InputField messageInputField;

        protected override void Awake()
        {
            base.Awake();
            
            // dialog style dropdown
            foreach (EDialogStyle style in Enum.GetValues(typeof(EDialogStyle)))
            {
                dialogStyleDropdown.options.Add(new TMP_Dropdown.OptionData(style.ToString()));    
            }
            
            // dialog size dropdown
            foreach (EDialogSize size in Enum.GetValues(typeof(EDialogSize)))
            {
                dialogSizeDropdown.options.Add(new TMP_Dropdown.OptionData(size.ToString()));    
            }
            dialogSizeDropdown.value = 1;
            
            // toast style dropdown
            foreach (EToastStyle style in Enum.GetValues(typeof(EToastStyle)))
            {
                toastStyleDropdown.options.Add(new TMP_Dropdown.OptionData(style.ToString()));    
            }
            
            // toast duration dropdown
            foreach (EToastDuration duration in Enum.GetValues(typeof(EToastDuration)))
            {
                toastDurationDropdown.options.Add(new TMP_Dropdown.OptionData(duration.ToString()));    
            }
            
        }

        private EToastStyle ToastStyle => (EToastStyle) toastStyleDropdown.value;
        private EToastDuration ToastDuration => (EToastDuration) toastDurationDropdown.value;
        private EDialogSize DialogSize => (EDialogSize) dialogSizeDropdown.value;
        private EDialogStyle DialogStyle => (EDialogStyle) dialogStyleDropdown.value;
        private string Title => titleInputField.text;
        private string Message => messageInputField.text;
        
        public void OnButtonClick_ShowCustomToast()
        {
            Toasts.ShowToast(
                new ToastContent()
                {
                    Title = Title,
                    Text = Message,
                    Style = ToastStyle,
                    Duration = Toasts.ToastDurationProvider.GetDuration(ToastDuration)
                }
            );
        }

        public void OnButtonClick_ShowSimpleMessage()
        {
            Dialogs.ShowMessageDialog(Title, Message, null, DialogSize, DialogStyle);
        }

        public void OnButtonClick_ShowYesNoConfirmationDialog()
        {
            Dialogs.ShowConfirmationDialog(
                Title,
                Message,
                (result) => { Toasts.ShowToast("Result", $"Result == {result}"); },
                Dialogs.EConfirmationDialogType.YesNo,
                DialogSize,
                DialogStyle
            );
        }

        public void OnButtonClick_ShowOkCancelConfirmationDialog()
        {
            Dialogs.ShowConfirmationDialog(
                Title,
                Message,
                (result) => { Toasts.ShowToast("Result", $"Result == {result}"); },
                Dialogs.EConfirmationDialogType.OkCancel,
                DialogSize,
                DialogStyle
            );
        }
        
    }

}
