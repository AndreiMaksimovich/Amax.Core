// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

#endregion

namespace Amax.Navigation
{
    
    internal class LoadSceneAsyncTask: ILoadSceneAsyncTask
    {

        private readonly Action _beforeSceneActivationCallback;

        public LoadSceneAsyncTask(MonoBehaviour coroutineRunner, OpenSceneIntent intent, Action beforeSceneActivationCallback = null)
        {
            OpenSceneIntent = intent;
            IsSceneActivationAllowed = intent.ActivateSceneImmediately;
            _beforeSceneActivationCallback = beforeSceneActivationCallback;
            coroutineRunner.StartCoroutine(LoadSceneCoroutine());
        }
        
        public OpenSceneIntent OpenSceneIntent { get; }

        public void ActivateScene()
            => IsSceneActivationAllowed = true;

        public bool IsSceneActivationAllowed { get; set; }
        public bool IsLoading { get; private set; }
        public float Progress { get; private set; }

        public event Action<ILoadSceneAsyncTask> OnSceneLoaded;
        public event Action<ILoadSceneAsyncTask> OnSceneLoadFailed;
        public event Action<ILoadSceneAsyncTask> OnSceneActivation;

        private IEnumerator LoadSceneCoroutine()
        {
            var scene = OpenSceneIntent.Scene;
            IsLoading = true;

            // Addressable
            if (scene.Type == Scene.ESceneType.Addressable)
            {
                Debug.Log("LoadSceneCoroutine: Start Addressable");
                var operationHandle = Addressables.LoadSceneAsync(scene.SceneAssetReference, LoadSceneMode.Single, false);

                while (!operationHandle.IsDone)
                {
                    Progress = operationHandle.PercentComplete;
                    yield return null;
                }

                if (operationHandle.Status == AsyncOperationStatus.Failed)
                {
                    OnSceneLoadFailed?.Invoke(this);
                }
                else
                {
                    OnSceneLoaded?.Invoke(this);

                    while (!IsSceneActivationAllowed)
                    {
                        yield return null;
                    }
                    
                    _beforeSceneActivationCallback?.Invoke();
                    OnSceneActivation?.Invoke(this);
                    yield return operationHandle.Result.ActivateAsync();
                }
                
                Addressables.Release(operationHandle);
            }
            // Built In Scene
            else
            {
                var asyncOperation = scene.Type == Scene.ESceneType.BuiltInBuildId
                    ? SceneManager.LoadSceneAsync(scene.BuildId, LoadSceneMode.Single)
                    : SceneManager.LoadSceneAsync(scene.BuildName, LoadSceneMode.Single);
                
                asyncOperation.allowSceneActivation = false;

                while (asyncOperation.progress < 0.9f)
                {
                    Progress = asyncOperation.progress;
                    yield return null;
                }
                
                OnSceneLoaded?.Invoke(this);

                while (!IsSceneActivationAllowed)
                {
                    yield return null;
                }
                
                _beforeSceneActivationCallback?.Invoke();
                OnSceneActivation?.Invoke(this);
                asyncOperation.allowSceneActivation = true;
            }
            
            IsLoading = false;
        }

    }
}