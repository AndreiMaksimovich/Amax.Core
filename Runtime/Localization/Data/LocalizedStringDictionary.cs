// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using UnityEngine;

#endregion

namespace Amax.Localization
{
    [Serializable]
    public class LocalizedStringDictionary : SerializableDictionary<SystemLanguage, string>
    {
        
        public string Get(SystemLanguage language, string defaultValue = null)
            => ContainsKey(language) ? this[language] : defaultValue;
        

        public string Get(SystemLanguage language, SystemLanguage defaultLanguage, string defaultValue = null)
        {
            if (ContainsKey(language)) return this[language];
            return ContainsKey(defaultLanguage) ? this[defaultLanguage] : defaultValue;
        }
        
    }
}