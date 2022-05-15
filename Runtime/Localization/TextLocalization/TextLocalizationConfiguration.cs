// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Amax.Localization
{
    
    [CreateAssetMenu(menuName = "Amax/Text.Localization/Configuration", fileName = "TextLocalization.Configuration.asset")]
    public class TextLocalizationConfiguration: ScriptableObject
    {
        [field: SerializeField] public bool ShowWarnings { get; private set; } = true;
        [field: SerializeField] public SystemLanguage DefaultLanguage { get; private set; } = SystemLanguage.English;
        [field: SerializeField] public SystemLanguage[] SupportedLanguages { get; private set; } = new SystemLanguage[]
        {
            SystemLanguage.English
        };
        [field: SerializeField] public List<ALocalizedStringProvider> GlobalLocalizedStringProviders { get; set; }
    }
    
}