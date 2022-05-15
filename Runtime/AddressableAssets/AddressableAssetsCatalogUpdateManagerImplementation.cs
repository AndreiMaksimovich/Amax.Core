// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#endregion

namespace Amax.AddressableAssets
{
    
    public class AddressableAssetsCatalogUpdateManagerImplementation : MonoBehaviour, IAddressableAssetsCatalogUpdateManager, IEventBusListener<OnSceneDestroy>
    {

        [field: SerializeField] public bool CheckForCatalogUpdateAutomatically { get; set; } = false;
        [field: SerializeField] public int CheckForCatalogUpdatePeriodicity { get; set; } = 24 * 3600;
        [field: SerializeField] public bool UpdateCatalogAutomatically { get; set; } = false;

        private long _lastCheckForCatalogUpdateTime;
        
        public bool IsCatalogUpdateAvailable { get; private set; }
        public event Action OnCatalogUpdateAvailable;
        public event Action OnCatalogUpdated;
        public event Action<Exception> OnCatalogUpdateFailed;

        private readonly List<Tuple<Action<ECheckForAddressableAssetsCatalogUpdateResult, Exception>, IAddressableAssetsCatalogUpdateManager.EContextType>> 
            _checkForCatalogUpdateCallbacks = new();

        private readonly List<Tuple<Action<bool, Exception>, IAddressableAssetsCatalogUpdateManager.EContextType>>
            _updateCatalogCallbacks = new ();
        
        private bool _isCheckForCatalogUpdateInProgress = false;
        private bool _isCatalogUpdateInProgress = false;
        
        public void CheckForCatalogUpdates(Action<ECheckForAddressableAssetsCatalogUpdateResult, Exception> callback = null, IAddressableAssetsCatalogUpdateManager.EContextType callbackContext = IAddressableAssetsCatalogUpdateManager.EContextType.Scene)
        {
            if (callback != null)
            {
                _checkForCatalogUpdateCallbacks.Add(new (callback, callbackContext));
            }
            if (!_isCheckForCatalogUpdateInProgress) StartCoroutine(CheckForCatalogUpdateCoroutine());
        }

        public void UpdateCatalog(Action<bool, Exception> callback = null, IAddressableAssetsCatalogUpdateManager.EContextType callbackContext = IAddressableAssetsCatalogUpdateManager.EContextType.Scene)
        {
            if (callback != null)
            {
                _updateCatalogCallbacks.Add(new(callback, callbackContext));
            }
            if (!_isCatalogUpdateInProgress) StartCoroutine(UpdateCatalogCoroutine());
        }

        private IEnumerator UpdateCatalogCoroutine()
        {
            _isCatalogUpdateInProgress = true;
            var task = Addressables.UpdateCatalogs(null, true);
            yield return task;

            foreach (var callback in _updateCatalogCallbacks)
            {
                callback.Item1?.Invoke(task.Status == AsyncOperationStatus.Succeeded, task.OperationException);
            }

            if (task.Status == AsyncOperationStatus.Succeeded)
            {
                OnCatalogUpdated?.Invoke();
            }
            else if (task.Status == AsyncOperationStatus.Failed)
            {
                OnCatalogUpdateFailed?.Invoke(task.OperationException);
            }
            
            _isCatalogUpdateInProgress = false;
        }

        private IEnumerator CheckForCatalogUpdateCoroutine()
        {
            _isCheckForCatalogUpdateInProgress = true;

            var task = Addressables.CheckForCatalogUpdates(true);
            yield return task;

            if (task.Status == AsyncOperationStatus.Failed || task.OperationException!=null)
            {
                EventBus.RaiseEvent(new OnAddressableAssetsCatalogUpdateCheckFailed() { Exception = task.OperationException });
                foreach (var checkForCatalogUpdateCallback in _checkForCatalogUpdateCallbacks)
                {
                    checkForCatalogUpdateCallback.Item1?.Invoke(ECheckForAddressableAssetsCatalogUpdateResult.Failed, task.OperationException);
                }
            }
            else
            {
                IsCatalogUpdateAvailable = task.Result != null && task.Result.Count > 0;
                if (IsCatalogUpdateAvailable)
                {
                    EventBus.RaiseEvent(new OnAddressableAssetsCatalogUpdateAvailable());
                }
                foreach (var callback in _checkForCatalogUpdateCallbacks)
                {
                    callback.Item1?.Invoke(IsCatalogUpdateAvailable ? ECheckForAddressableAssetsCatalogUpdateResult.UpdateAvailable : ECheckForAddressableAssetsCatalogUpdateResult.UpdateNotAvailable, null);
                }
                OnCatalogUpdateAvailable?.Invoke();
                _lastCheckForCatalogUpdateTime = UnixTime.Now;
            }
            
            _checkForCatalogUpdateCallbacks.Clear();
            _isCheckForCatalogUpdateInProgress = false;

            if (IsCatalogUpdateAvailable && UpdateCatalogAutomatically)
            {
                UpdateCatalog();
            }
        }


        public void OnEvent(OnSceneDestroy data)
        {
            // Check For Catalog Update Callbacks
            var checkForCatalogUpdateCallbacksToRemove =
                new List<Tuple<Action<ECheckForAddressableAssetsCatalogUpdateResult, Exception>,
                    IAddressableAssetsCatalogUpdateManager.EContextType>>();
            foreach (var callback in _checkForCatalogUpdateCallbacks)
            {
                if (callback.Item2 == IAddressableAssetsCatalogUpdateManager.EContextType.Scene)
                {
                    checkForCatalogUpdateCallbacksToRemove.Add(callback);
                }
            }
            foreach (var callback in checkForCatalogUpdateCallbacksToRemove)
            {
                _checkForCatalogUpdateCallbacks.Remove(callback);
            }
            
            // Catalog Update Callbacks
            var updateCatalogCallbacksToRemove =
                new List<Tuple<Action<bool, Exception>, IAddressableAssetsCatalogUpdateManager.EContextType>>();
            foreach (var callback in _updateCatalogCallbacks)
            {
                if (callback.Item2 == IAddressableAssetsCatalogUpdateManager.EContextType.Scene)
                {
                    updateCatalogCallbacksToRemove.Add(callback);
                }
            }
            foreach (var callback in updateCatalogCallbacksToRemove)
            {
                _updateCatalogCallbacks.Remove(callback);
            }
        }

        private void Start()
        {
            InvokeRepeating(nameof(AutomaticCheckForCatalogUpdate), 3600, 3600);
        }

        private void AutomaticCheckForCatalogUpdate()
        {
            if (CheckForCatalogUpdateAutomatically && UnixTime.Now - _lastCheckForCatalogUpdateTime > CheckForCatalogUpdatePeriodicity)
            {
                CheckForCatalogUpdates();
            }
        }

        private void Awake()
        {
            _lastCheckForCatalogUpdateTime = UnixTime.Now;
            EventBus.AddListener(this as IEventBusListener<OnSceneDestroy>);
        }
        
        private void OnDestroy()
        {
            EventBus.RemoveListener(this as IEventBusListener<OnSceneDestroy>);
        }
        
    }


}
