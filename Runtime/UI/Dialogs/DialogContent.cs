// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Amax.UI.Dialogs
{
    
    public class DialogContent: IDialogContent
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public Sprite Image { get; set; }
        public GameObject Content { get; set; }
        public GameObject ContentPrefab { get; set; }
        public GameObject DialogPrefab { get; set; }
        public EDialogStyle Style { get; set; } = EDialogStyle.Default;
        public EDialogSize Size { get; set; } = EDialogSize.Medium;
        public List<IDialogButtonContent> Buttons { get; set; } = new List<IDialogButtonContent>();
    }
    
}