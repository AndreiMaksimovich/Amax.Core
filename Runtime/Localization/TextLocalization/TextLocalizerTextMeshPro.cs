// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using TMPro;
using UnityEngine;

#endregion

namespace Amax.Localization
{
    [RequireComponent(typeof(TextMeshPro))]
    public class TextLocalizerTextMeshPro: ATextLocalizer
    {

        private TextMeshPro _textMeshPro;

        private TextMeshPro TextMeshPro
        {
            get
            {
                if (_textMeshPro == null)
                {
                    _textMeshPro = GetComponent<TextMeshPro>();
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