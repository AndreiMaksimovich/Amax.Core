// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Amax.AddressableAssets
{
    [Serializable]
    public class AssetReferenceSprite: AssetReferenceT<Sprite>
    {
        public AssetReferenceSprite(string guid) : base(guid) { }
    }
}