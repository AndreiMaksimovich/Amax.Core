// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Amax.Localization
{
    [CreateAssetMenu(menuName = "Amax/Localization/LocalizedStringGroup", fileName = "LocalizedStringsGroup.asset", order = -1)]
    public class LocalizedStringProviderGroup: ALocalizedStringProvider
    {
        
        [field: SerializeField] public override string Id { get; set; }
        [field: SerializeField] public string StringIdPrefix { get; set; }
        [field: SerializeField] public List<ALocalizedStringProvider> LocalizedStringProviders { get; set; }
        
        public override void GetStrings(Action<string, Dictionary<SystemLanguage, string>> processString)
        {
            foreach (var locStrProvider in LocalizedStringProviders)
            {
                locStrProvider.GetStrings(
                    (id, strings) =>
                    {
                        processString.Invoke(LocalizedStringIdUtils.Join(StringIdPrefix, id), strings);
                    }
                );
            }
        }
        
    }
}