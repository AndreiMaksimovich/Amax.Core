// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using UnityEngine;
using UnityEngine.UI;

#endregion

namespace Amax.Localization
{
    
    [RequireComponent(typeof(Text))]
    public class TextLocalizerUIText: ATextLocalizer
    {

        private Text _text;

        private Text Text
        {
            get
            {
                if (_text == null)
                {
                    _text = GetComponent<Text>();
                }
                return _text;
            }
        }
        
        public override void Refresh()
        {
            Text.text = TextLocalization.Get(FullStringId);
        }
        
    }
}