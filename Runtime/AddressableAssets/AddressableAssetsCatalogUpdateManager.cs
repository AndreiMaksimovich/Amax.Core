// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using UnityEngine;

#endregion

namespace Amax.AddressableAssets
{
    public class AddressableAssetsCatalogUpdateManager: MonoBehaviour
    {
        public static IAddressableAssetsCatalogUpdateManager Instance { get; set; }

        private void Awake()
        {
            Instance ??= GetComponent<IAddressableAssetsCatalogUpdateManager>();
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public static bool IsCatalogUpdateAvailable => Instance.IsCatalogUpdateAvailable;

        public static void CheckForCatalogUpdates(
            Action<ECheckForAddressableAssetsCatalogUpdateResult, Exception> callback = null,
            IAddressableAssetsCatalogUpdateManager.EContextType callbackContext = IAddressableAssetsCatalogUpdateManager.EContextType.Scene)
            => Instance.CheckForCatalogUpdates(callback, callbackContext);

        public static void UpdateCatalog(Action<bool, Exception> callback = null,
            IAddressableAssetsCatalogUpdateManager.EContextType callbackContext =
                IAddressableAssetsCatalogUpdateManager.EContextType.Scene)
            => Instance.UpdateCatalog(callback, callbackContext);
        
        public static event Action OnCatalogUpdateAvailable
        {
            add => Instance.OnCatalogUpdateAvailable += value;
            remove => Instance.OnCatalogUpdateAvailable -= value;
        }
        
        public static event Action OnCatalogUpdated
        {
            add => Instance.OnCatalogUpdated += value;
            remove => Instance.OnCatalogUpdated -= value;
        }
        
        public static event Action<Exception> OnCatalogUpdateFailed
        {
            add => Instance.OnCatalogUpdateFailed += value;
            remove => Instance.OnCatalogUpdateFailed -= value;
        }
        
    }
}