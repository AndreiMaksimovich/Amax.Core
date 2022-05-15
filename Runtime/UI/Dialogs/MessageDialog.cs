// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;

#endregion

namespace Amax.UI.Dialogs
{
    public class MessageDialog: ADialogBase, IMessageDialog
    {
        protected override void Configure(IDialogView dialogView)
        {
            dialogView.OnDialogButtonClick += code =>
            {
                Close();
                OnClose?.Invoke(this);
            };
        }

        public event Action<IMessageDialog> OnClose;

        public MessageDialog(IDialogView dialogView) : base(dialogView) { }
        
    }
}