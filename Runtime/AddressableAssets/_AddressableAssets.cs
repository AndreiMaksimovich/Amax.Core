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

    public class AssetReferenceRuntimeKeyInvalidException : Exception
    {
        public AssetReference AssetReference { get; set; }
        public AssetReferenceRuntimeKeyInvalidException(string message, AssetReference assetReference = null) :
            base(message)
        {
            AssetReference = assetReference;
        }
    }

    
    
    public interface IAddressableAssetsManager
    {
        public ILoadAddressableAssetTask Get<T>(AssetReference assetReference, Action<T, Exception> callback = null, EAddressableAssetReleaseCondition releaseCondition = EAddressableAssetReleaseCondition.OnSceneDestroy, EContextType context = EContextType.Scene, Func<AssetReference, bool> customReleaseCondition = null) where T : Object;
        public ILoadAddressableAssetTask Instantiate(AssetReference assetReference, Action<GameObject, Exception> callback = null, EAddressableAssetReleaseCondition releaseCondition = EAddressableAssetReleaseCondition.OnSceneDestroy, EContextType context = EContextType.Scene, Func<AssetReference, bool> customReleaseCondition = null);
        
        public void ReleaseInstance(AssetReference assetReference, GameObject assetInstance);
        public void Release(AssetReference assetReference);
        
        public enum EContextType
        {
            Scene,
            Global
        }
    }
    
    public interface ILoadAddressableAssetTask
    {
        bool IsDone { get; }
        bool IsCanceled { get; }
        Exception Exception { get; }
        Object Result { get; }
        void Cancel();
    }
    
    public enum EAddressableAssetReleaseCondition
    {
        Custom,
        Manual,
        OnSceneDestroy,
        OnNextSceneDestroy,
    }
    
    public interface IAddressableAssetsCatalogUpdateManager
    {
        public bool IsCatalogUpdateAvailable { get; }
        public event Action OnCatalogUpdateAvailable;
        public event Action OnCatalogUpdated;
        public event Action<Exception> OnCatalogUpdateFailed;

        public void CheckForCatalogUpdates(Action<ECheckForAddressableAssetsCatalogUpdateResult, Exception> callback = null, EContextType callbackContext = EContextType.Scene);

        public void UpdateCatalog(Action<bool, Exception> callback = null, EContextType callbackContext = EContextType.Scene);
        
        public enum EContextType
        {
            Scene,
            Global
        }
    }
    
    public class OnAddressableAssetsCatalogUpdateAvailable : EventBusBaseEvent {}

    public class OnAddressableAssetsCatalogUpdateCheckFailed : EventBusBaseEvent
    {
        public Exception Exception { get; set; }
    }
    
    public enum ECheckForAddressableAssetsCatalogUpdateResult
    {
        UpdateAvailable,
        UpdateNotAvailable,
        Failed
    }

}
