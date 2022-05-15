// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using Amax.Localization;
using TMPro;
using UnityEngine;

#endregion

namespace Amax.Core.Examples
{

    public class SceneControllerExampleTextLocalization : MonoBehaviour
    {

        public LocalizedAssetReferenceAudioClipDictionary test;
        
        [SerializeField] private TMP_Dropdown languageDropdown;

        [SerializeField] private SystemLanguage[] supportedLanguages =
            {SystemLanguage.English, SystemLanguage.Polish, SystemLanguage.Russian};

        private void Start()
        {
            languageDropdown.onValueChanged.AddListener(
                index => { TextLocalization.Language = supportedLanguages[index]; }
            );
        }
    }

}
