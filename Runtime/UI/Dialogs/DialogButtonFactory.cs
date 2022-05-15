// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using Amax.Localization;
using UnityEngine;

#endregion

namespace Amax.UI.Dialogs
{
    public class DialogButtonFactory: MonoBehaviour, IDialogButtonFactory
    {

        [field: SerializeField] public string ButtonLabelLocalizationPrefix { get; set; } = "Amax.UI.Dialogs.ButtonLabel";

        private string GetButtonLabel(EDialogButtonType type)
            => TextLocalization.Get(ButtonLabelLocalizationPrefix, type.ToString());

        public IDialogButtonContent GetButton(EDialogButtonType type)
            => GetButton
            (
                GetButtonLabel(type),
                (int) type
            );

        public IDialogButtonContent GetButton(string title, int buttonCode, Sprite icon = null, EDialogButtonState state = EDialogButtonState.Enabled)
            => new DialogButtonContent()
            {
                Label = title,
                Code = buttonCode,
                Icon = icon,
                State = state
            };
        
    }
}