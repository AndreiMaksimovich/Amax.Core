// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

#endregion

namespace Amax.AddressableAssets
{
    public class AddressableAssetsManager: MonoBehaviour
    {
        
        public static IAddressableAssetsManager Instance { get; set; }

        private void Awake()
        {
            Instance ??= GetComponent<IAddressableAssetsManager>();
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public static ILoadAddressableAssetTask Get<T>(AssetReference assetReference,
            Action<T, Exception> callback = null,
            EAddressableAssetReleaseCondition releaseCondition = EAddressableAssetReleaseCondition.OnSceneDestroy,
            IAddressableAssetsManager.EContextType context = IAddressableAssetsManager.EContextType.Scene, Func<AssetReference, bool> customReleaseCondition = null)
            where T : Object
            => Instance.Get<T>(assetReference, callback, releaseCondition, context, customReleaseCondition);

        public static ILoadAddressableAssetTask Instantiate(AssetReference assetReference,
            Action<GameObject, Exception> callback = null,
            EAddressableAssetReleaseCondition releaseCondition = EAddressableAssetReleaseCondition.OnSceneDestroy,
            IAddressableAssetsManager.EContextType context = IAddressableAssetsManager.EContextType.Scene, Func<AssetReference, bool> customReleaseCondition = null)
            => Instance.Instantiate(assetReference, callback, releaseCondition, context, customReleaseCondition);

        public static void ReleaseInstance(AssetReference assetReference, GameObject assetInstance)
            => Instance.ReleaseInstance(assetReference, assetInstance);

        public static void Release(AssetReference assetReference)
            => Instance.Release(assetReference);

    }
}