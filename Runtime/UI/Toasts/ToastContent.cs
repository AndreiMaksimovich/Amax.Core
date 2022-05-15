// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using UnityEngine;

#endregion

namespace Amax.UI.Toasts
{
    public class ToastContent: IToastContent
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public float Duration { get; set; }
        public Sprite Icon { get; set; }
        public EToastStyle Style { get; set; }
        public GameObject Content { get; set; }
        public GameObject ContentPrefab { get; set; }
    }
}