// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using UnityEngine;

#endregion

namespace Amax.Localization
{
    
    [Serializable]
    public class LocalizedDictionary<T>: SerializableDictionary<SystemLanguage, T> where T: class
    {

        public T Get(SystemLanguage language, T defaultValue = null)
            => ContainsKey(language) ? this[language] : defaultValue;

        public T Get(SystemLanguage language, SystemLanguage defaultLanguage, T defaultValue = null)
        {
            if (ContainsKey(language)) return this[language];
            return ContainsKey(defaultLanguage) ? this[defaultLanguage] : defaultValue;
        }
        
    }
    
}