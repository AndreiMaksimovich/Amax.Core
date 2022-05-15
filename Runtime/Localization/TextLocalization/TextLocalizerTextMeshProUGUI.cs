// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using TMPro;
using UnityEngine;

#endregion

namespace Amax.Localization
{
    
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextLocalizerTextMeshProUGUI: ATextLocalizer
    {

        private TextMeshProUGUI _textMeshPro;

        private TextMeshProUGUI TextMeshPro
        {
            get
            {
                if (_textMeshPro == null)
                {
                    _textMeshPro = GetComponent<TextMeshProUGUI>();
                }
                return _textMeshPro;
            }
        }
        
        public override void Refresh()
        {
            TextMeshPro.text = TextLocalization.Get(FullStringId);
        }
        
    }
    
}