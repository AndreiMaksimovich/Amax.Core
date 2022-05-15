// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Amax.UI.Dialogs
{
    public class Dialogs: MonoBehaviour
    {
        
        public static IDialogs Instance { get; set; }

        private void Awake()
        {
            Instance ??= GetComponent<IDialogs>();
        }
        
        private void OnDestroy()
        {
            Instance = null;
        }

        public static IMessageDialog ShowMessageDialog(IDialogContent dialogContent) => Instance.ShowMessageDialog(dialogContent);

        public static IResultDialog ShowResultDialog(IDialogContent dialogContent) => Instance.ShowResultDialog(dialogContent);
        
        public static IDialogButtonFactory DialogButtonFactory  => Instance.DialogButtonFactory;

        public static void ShowMessageDialog(string title, string text, Action onClose = null, EDialogSize size = EDialogSize.Medium, EDialogStyle style = EDialogStyle.Default, Sprite image = null)
        {
            var dialogContent = new DialogContent()
            {
                Title = title,
                Text = text,
                Style = style,
                Size = size,
                Image = image,
                Buttons = {Instance.DialogButtonFactory.GetButton(EDialogButtonType.OK)}
            };
            var dialog = ShowMessageDialog(dialogContent);
            if (onClose != null) dialog.OnClose += _ => onClose.Invoke();
        }

        public static void ShowWarningMessageDialog(string title, string text, Action onClose = null,
            EDialogSize size = EDialogSize.Medium)
            => ShowMessageDialog(title, text, onClose, size, EDialogStyle.Warning);
        
        public static void ShowErrorMessageDialog(string title, string text, Action onClose = null,
            EDialogSize size = EDialogSize.Medium)
            => ShowMessageDialog(title, text, onClose, size, EDialogStyle.Error);

        public static void ShowResultDialog(string title, string text, List<IDialogButtonContent> buttons, Action<int> onClose, EDialogSize size = EDialogSize.Medium, EDialogStyle style = EDialogStyle.Default, Sprite image = null)
        {
            var dialogContent = new DialogContent()
            {
                Title = title,
                Text = text,
                Style = style,
                Size = size,
                Image = image,
                Buttons = buttons
            };
            var dialog = ShowResultDialog(dialogContent);
            dialog.OnClose += (resultDialog, resultCode) => onClose.Invoke(resultCode);
        }

        public static void ShowConfirmationDialog(string title, string text, Action<bool> onClose, EConfirmationDialogType type = EConfirmationDialogType.OkCancel, EDialogSize size = EDialogSize.Medium, EDialogStyle style = EDialogStyle.Default, Sprite image = null)
        {
            var dialogContent = new DialogContent()
            {
                Title = title,
                Text = text,
                Style = style,
                Image = image,
                Size = size,
                Buttons = type == EConfirmationDialogType.OkCancel 
                    ? 
                    new List<IDialogButtonContent>() {DialogButtonFactory.GetButton(EDialogButtonType.OK), DialogButtonFactory.GetButton(EDialogButtonType.Cancel)} 
                    : 
                    new List<IDialogButtonContent>() {DialogButtonFactory.GetButton(EDialogButtonType.Yes), DialogButtonFactory.GetButton(EDialogButtonType.No)}
            };
            var dialog = ShowResultDialog(dialogContent);
            dialog.OnClose += (resultDialog, resultCode) =>
            {
                if (type == EConfirmationDialogType.YesNo)
                {
                    onClose.Invoke(resultCode == (int) EDialogButtonType.Yes);
                }
                else
                {
                    onClose.Invoke(resultCode == (int) EDialogButtonType.OK);
                }
            };
        }
        
        public enum EConfirmationDialogType
        {
            YesNo,
            OkCancel
        }

    }
}