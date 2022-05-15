// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

using UnityEngine.AddressableAssets;
using UnityEngine;

namespace Amax.Localization
{

    public class LocalizedAssetReferenceTDictionary<T> : SerializableDictionary<SystemLanguage, AssetReferenceT<T>>
        where T : Object
    {

        public AssetReferenceT<T> Get(SystemLanguage language = SystemLanguage.Unknown,
            SystemLanguage defaultLanguage = SystemLanguage.Unknown, AssetReferenceT<T> defaultValue = null)
        {
            if (ContainsKey(language)) return this[language];
            if (ContainsKey(defaultLanguage)) return this[defaultLanguage];
            return defaultValue;
        }
        
    }
    
}