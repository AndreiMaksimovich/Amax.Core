// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

namespace Amax.UI.Dialogs
{
    public abstract class ADialogBase: IDialogBase
    {
        
        private IDialogView DialogView { get; set; }

        public ADialogBase(IDialogView dialogView)
        {
            DialogView = dialogView;
            Configure(dialogView);
        }

        protected abstract void Configure(IDialogView dialogView);
        
        public bool IsOpened { get; protected set; } = true;
        
        public void Close()
        {
            if (!IsOpened) return;
            IsOpened = false;
            DialogView.Close();
        }
        
    }
}