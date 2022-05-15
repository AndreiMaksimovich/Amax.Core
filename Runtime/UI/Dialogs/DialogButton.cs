// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using UnityEngine;

#endregion

namespace Amax.UI.Dialogs
{
    public class DialogButtonContent: IDialogButtonContent
    {
        public int Code { get; set; }
        public EDialogButtonState State { get; set; } = EDialogButtonState.Enabled;
        public string Label { get; set; }
        public Sprite Icon { get; set; }
    }
}