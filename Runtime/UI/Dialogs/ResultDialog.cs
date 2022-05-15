// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;

#endregion

namespace Amax.UI.Dialogs
{
    public class ResultDialog: ADialogBase, IResultDialog
    {
        
        protected override void Configure(IDialogView dialogView)
        {
            dialogView.OnDialogButtonClick += buttonCode =>
            {
                ResultCode = buttonCode;
                Close();
                OnClose?.Invoke(this, ResultCode);
            };
        }

        public int ResultCode { get; private set; } = -1;
        public event Action<IResultDialog, int> OnClose;

        public ResultDialog(IDialogView dialogView) : base(dialogView) { }
        
    }
}