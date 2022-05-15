// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

#endregion

namespace Amax.AddressableAssets
{

    public class AddressableAssetsManagerImplementation : MonoBehaviour, IAddressableAssetsManager, IEventBusListener<OnSceneStart>, IEventBusListener<OnSceneDestroy>
    {
        protected const string LogTag = "AmaxAddressablesImplementation: ";

        private ILoadAddressableAssetTask CreateTaskHandlerForInvalidAssetReferenceRuntimeKeyError(AssetReference assetReference) =>
            new TaskHandler()
            {
                Exception = new AssetReferenceRuntimeKeyInvalidException("Asset reference run time key is invalid", assetReference),
                Result = null,
                IsDone = true
            };
        
        public ILoadAddressableAssetTask Get<T>(AssetReference assetReference, Action<T, Exception> callback = null, EAddressableAssetReleaseCondition releaseCondition = EAddressableAssetReleaseCondition.OnSceneDestroy, IAddressableAssetsManager.EContextType context = IAddressableAssetsManager.EContextType.Scene, Func<AssetReference, bool> customReleaseCondition = null) where T : Object
        {
            if (!assetReference.RuntimeKeyIsValid())
            {
                var handler = CreateTaskHandlerForInvalidAssetReferenceRuntimeKeyError(assetReference);
                callback?.Invoke(null, handler.Exception);
                return handler;
            }
            var record = GetOrCreateRecord<T>(assetReference, releaseCondition, customReleaseCondition);
            
            // Asset is loaded
            if (record.asset != null)
            {
                callback?.Invoke(record.asset as T, null);
                return new TaskHandler()
                {
                    Result = record.Asset,
                    IsDone = true
                };
            }

            var taskHandler = new TaskHandler();
            record.getAssetCallbacks.Add(
                new GetAssetCallbackRecord()
                {
                    context = context,
                    callback = (asset, ex) => callback?.Invoke(asset as T, ex),
                    taskHandler = taskHandler
                }
            );
            if (record.getAssetCallbacks.Count == 1)
            {
                record.getAssetCoroutine = GetAssetCoroutine(record);
                StartCoroutine(record.getAssetCoroutine);
            }
            return taskHandler;
        }

        public ILoadAddressableAssetTask Instantiate(AssetReference assetReference, Action<GameObject, Exception> callback = null, EAddressableAssetReleaseCondition releaseCondition = EAddressableAssetReleaseCondition.OnSceneDestroy, IAddressableAssetsManager.EContextType context = IAddressableAssetsManager.EContextType.Scene, Func<AssetReference, bool> customReleaseCondition = null)
        {
            var record = GetOrCreateRecord<GameObject>(assetReference, releaseCondition, customReleaseCondition);
            var taskHandler = new TaskHandler();
            var callbackRecord = new InstantiateAssetInstanceCallbackRecord()
            {
                callback = (assetInstance, exception) => callback?.Invoke(assetInstance, exception),
                context = context,
                taskHandler = taskHandler
            };
            callbackRecord.instantiateCoroutine = InstantiateAssetCoroutine(record, callbackRecord, releaseCondition);
            record.instantiateAssetInstanceCallbacks.Add(callbackRecord);
            StartCoroutine(callbackRecord.instantiateCoroutine);
            return taskHandler;
        }

        public void ReleaseInstance(AssetReference assetReference, GameObject assetInstance)
        {
            var record = GetRecord<Object>(assetReference);
            if (record==null) return;
            record.assetReference.ReleaseInstance(assetInstance);
            AssetReferenceAssetInstance toRemove = null;
            foreach (var assetReferenceAssetInstance in record.instances)
            {
                if (assetReferenceAssetInstance.instance == assetInstance)
                {
                    toRemove = assetReferenceAssetInstance;
                    break;
                }
            }
            if (toRemove != null) record.instances.Remove(toRemove);
        }

        private IEnumerator InstantiateAssetCoroutine(AssetReferenceRecord<GameObject> record, InstantiateAssetInstanceCallbackRecord callbackRecord, EAddressableAssetReleaseCondition releaseCondition)
        {
            var task = record.assetReference.InstantiateAsync();
            yield return task;
            GameObject result = null;
            Exception ex = null;
            
            result = task.Result;
            ex = task.OperationException;
            
            if (callbackRecord.taskHandler.IsCanceled)
            {
                if (result!=null) record.assetReference.ReleaseInstance(result);
            }
            else
            {
                callbackRecord.taskHandler.Result = result;
                callbackRecord.taskHandler.Exception = ex;
                callbackRecord.callback?.Invoke(result, ex);
                if (result!=null) {
                    record.instances.Add
                    (
                        new AssetReferenceAssetInstance()
                        {
                            instance = result,
                            releaseCondition = releaseCondition
                        }
                    );
                }
            }
            callbackRecord.taskHandler.IsDone = true;
            record.instantiateAssetInstanceCallbacks.Remove(callbackRecord);
        }
        
        private IEnumerator GetAssetCoroutine<T>(AssetReferenceRecord<T> record) where T : Object
        {
            var task = record.assetReference.LoadAssetAsync<T>();
            
            yield return task;
            
            T result = null;
            Exception ex = null;
            
            if (task.OperationException == null)
            {
                record.asset = task.Result;
                result = task.Result;
            }
            else
            {
                ex = task.OperationException;
            }
            
            var activeCallbacks = 0;
            foreach (var callback in record.getAssetCallbacks)
            {
                callback.taskHandler.IsDone = true;
                if (callback.taskHandler.IsCanceled) continue;
                activeCallbacks++;
                callback.callback?.Invoke(result, ex);
                callback.taskHandler.Result = result;
                callback.taskHandler.Exception = ex;
            }
            record.getAssetCallbacks.Clear();
            
            // Release if all callbacks are canceled
            if (activeCallbacks == 0 && result!=null)
            {
                record.assetReference.ReleaseAsset();
                record.asset = null;
            }
        }

        public void Release(AssetReference assetReference)
        {
            var record = GetRecord<Object>(assetReference);
            if (record == null) return;
            RemoveRecord(assetReference);
        }

        private AssetReferenceRecord<T> GetOrCreateRecord<T>(AssetReference assetReference, EAddressableAssetReleaseCondition releaseCondition = EAddressableAssetReleaseCondition.OnSceneDestroy, Func<AssetReference, bool> customReleaseCondition = null) where T : Object
        {
            var record = GetRecord<T>(assetReference);
            if (record == null)
            {
                record = new AssetReferenceRecord<T>()
                {
                    assetReference = assetReference,
                    releaseCondition = releaseCondition,
                    type = typeof(T),
                    customReleaseCondition = customReleaseCondition
                };
                Records.Add(GetAssetReferenceId(assetReference), record);
            }
            return record;
        }
        
        private void RemoveRecord(AssetReference assetReference)
        {
            Records.Remove(GetAssetReferenceId(assetReference));
        }
        
        private AssetReferenceRecord<T> GetRecord<T>(AssetReference assetReference) where T: Object
        {
            var id = GetAssetReferenceId(assetReference);
            if (Records.ContainsKey(id))
            {
                return Records[id] as AssetReferenceRecord<T>;
            }
            return null;
        }
        
        private static object GetAssetReferenceId(AssetReference assetReference) => assetReference.RuntimeKey;
        
        // -----------------------------------------------------------------------------

        private void Awake()
        {
            EventBus.AddListener(this as IEventBusListener<OnSceneStart>);
            EventBus.AddListener(this as IEventBusListener<OnSceneDestroy>);
        }

        private void OnDestroy()
        {
            EventBus.RemoveListener(this as IEventBusListener<OnSceneStart>);
            EventBus.RemoveListener(this as IEventBusListener<OnSceneDestroy>);
        }

        // -----------------------------------------------------------------------------
        
        public void OnEvent(OnSceneStart data)
        {
            OnSceneStart();
        }
        
        public void OnEvent(OnSceneDestroy data)
        {
            OnSceneDestroy();
        }

        private void OnSceneStart()
        {
            // Records
            foreach (var assetReferenceRecord in Records.Values)
            {
                if (assetReferenceRecord.releaseCondition == EAddressableAssetReleaseCondition.OnNextSceneDestroy)
                {
                    assetReferenceRecord.releaseCondition = EAddressableAssetReleaseCondition.OnSceneDestroy;
                }
            }
            // Instances
            foreach (var records in Records.Values)
            {
                foreach (var instance in records.instances)
                {
                    if (instance.releaseCondition == EAddressableAssetReleaseCondition.OnSceneDestroy)
                    {
                        instance.releaseCondition = EAddressableAssetReleaseCondition.OnSceneDestroy;
                    }
                }
            }
        }
        
        private void OnSceneDestroy()
        {
            
            var recordsToRemove = new List<AssetReferenceRecord>();
            foreach (var record in Records.Values)
            {
                if 
                (
                    record.releaseCondition == EAddressableAssetReleaseCondition.OnSceneDestroy 
                    || 
                    (record.releaseCondition == EAddressableAssetReleaseCondition.Custom && record.customReleaseCondition.Invoke(record.assetReference))
                )
                {
                    recordsToRemove.Add(record);
                }
            }
            
            foreach (var record in recordsToRemove)
            {
                ReleaseRecord(record);
                Records.Remove(GetAssetReferenceId(record.assetReference));
            }

            // Stop load & clear callbacks
            foreach (var record in Records.Values)
            {
                // Get Asset
                if (record.getAssetCallbacks.Count>0)
                {
                    var callbacksToRemove = new List<GetAssetCallbackRecord>();
                    foreach (var getAssetCallback in record.getAssetCallbacks.Where(getAssetCallback => getAssetCallback.context == IAddressableAssetsManager.EContextType.Scene))
                    {
                        callbacksToRemove.Add(getAssetCallback);
                    }
                    foreach (var callbackToRemove in callbacksToRemove)
                    {
                        callbackToRemove.taskHandler.Cancel();
                        record.getAssetCallbacks.Remove(callbackToRemove);
                    }
                }
                // Instantiate Asset Instance
                if (record.instantiateAssetInstanceCallbacks.Count > 0)
                {
                    var callbacksToRemove = new List<InstantiateAssetInstanceCallbackRecord>();
                    foreach (var instantiateAssetInstanceCallback in record.instantiateAssetInstanceCallbacks.Where(getAssetCallback => getAssetCallback.context == IAddressableAssetsManager.EContextType.Scene))
                    {
                        callbacksToRemove.Add(instantiateAssetInstanceCallback);
                        
                    }
                    foreach (var callbackToRemove in callbacksToRemove)
                    {
                        callbackToRemove.taskHandler.Cancel();
                        record.instantiateAssetInstanceCallbacks.Remove(callbackToRemove);
                    }
                }
            }
            
            // Release Local Instances
            var instancesToRelease = new List<Tuple<AssetReferenceRecord, GameObject>>();
            foreach (var record in Records.Values)
            {
                foreach (var assetInstance in record.instances)
                {
                    if (assetInstance.releaseCondition == EAddressableAssetReleaseCondition.OnSceneDestroy)
                    {
                        instancesToRelease.Add(new Tuple<AssetReferenceRecord, GameObject>(record, assetInstance.instance));
                    }
                }
            }
            foreach (var instanceToRelease in instancesToRelease)
            {
                ReleaseInstance(instanceToRelease.Item1.assetReference, instanceToRelease.Item2);
            }
            
        }

        private void ReleaseRecord(AssetReferenceRecord record)
        {
            
            StopCoroutine(record.getAssetCoroutine);
            
            // instances
            foreach (var assetInstance in record.instances)
            {
                record.assetReference.ReleaseInstance(assetInstance.instance);
            }
            
            // cancel Instantiate Instance callbacks
            foreach (var instantiateAssetCallback in record.instantiateAssetInstanceCallbacks)
            {
                instantiateAssetCallback.taskHandler.Cancel();
            }
            
            // cancel Get Asset callbacks
            foreach (var getAssetCallback in record.getAssetCallbacks)
            {
                getAssetCallback.taskHandler.Cancel();
            }
            
            // release Asset
            if (record.asset != null)
            {
                record.assetReference.ReleaseAsset();
            }
            
        }
        
        // -----------------------------------------------------------------------------
        
        private Dictionary<object, AssetReferenceRecord> Records { get; } = new Dictionary<object, AssetReferenceRecord>();
        
        protected class InstantiateAssetInstanceCallbackRecord
        {
            public IAddressableAssetsManager.EContextType context = IAddressableAssetsManager.EContextType.Scene;
            public Action<GameObject, Exception> callback;
            public IEnumerator instantiateCoroutine;
            public TaskHandler taskHandler;
        }
        
        protected class GetAssetCallbackRecord
        {
            public IAddressableAssetsManager.EContextType context = IAddressableAssetsManager.EContextType.Scene;
            public Action<Object, Exception> callback;
            public TaskHandler taskHandler;
        }
        
        protected class AssetReferenceAssetInstance
        {
            public GameObject instance;
            public EAddressableAssetReleaseCondition releaseCondition = EAddressableAssetReleaseCondition.OnSceneDestroy;
        }
        
        protected class AssetReferenceRecord
        {
            public Object asset;
            public readonly List<GetAssetCallbackRecord> getAssetCallbacks = new List<GetAssetCallbackRecord>();
            public Type type;
            public AssetReference assetReference;
            public EAddressableAssetReleaseCondition releaseCondition = EAddressableAssetReleaseCondition.OnSceneDestroy;
            public readonly List<AssetReferenceAssetInstance> instances = new List<AssetReferenceAssetInstance>();
            public readonly List<InstantiateAssetInstanceCallbackRecord> instantiateAssetInstanceCallbacks = new List<InstantiateAssetInstanceCallbackRecord>();
            public IEnumerator getAssetCoroutine;
            public Func<AssetReference, bool> customReleaseCondition;
        }
        
        protected class AssetReferenceRecord<T>: AssetReferenceRecord where T: Object
        {
            public T Asset => asset as T;
        }
        
        protected class TaskHandler : ILoadAddressableAssetTask
        {
            public bool IsDone { get; set; } = false;
            public Exception Exception { get; set; }
            public Object Result { get; set; }

            public bool IsCanceled { get; protected set; } = false;

            public void Cancel()
            {
                IsCanceled = true;
            }
        }

    }

}
