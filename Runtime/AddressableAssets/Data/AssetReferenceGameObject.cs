// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

#endregion

namespace Amax.AddressableAssets
{

    [Serializable] 
    public class AssetReferenceGameObject : AssetReferenceT<GameObject>
    {
        public AssetReferenceGameObject(string guid) : base(guid) { }
    }
    
}