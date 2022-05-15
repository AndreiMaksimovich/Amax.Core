// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using UnityEngine;

#endregion

namespace Amax.Web
{
    internal abstract class AWebLoaderTaskDeserializer<T> : IWebLoaderTask<T> where T : class
    {

        private readonly WebLoaderTask<string> _stringLoaderTask;
        public event Action OnTaskCoroutineFinished;
        
        protected AWebLoaderTaskDeserializer(MonoBehaviour coroutineRunner, IWebLoaderDataProcessorProvider dataProcessorProvider, string url, WWWForm wwwForm, Action<T, Exception> onComplete)
        {
            if (onComplete!=null) OnComplete += onComplete;
            _stringLoaderTask = new WebLoaderTask<string>(coroutineRunner, dataProcessorProvider, url, wwwForm, OnStringLoaderTaskComplete);
        }

        private void OnStringLoaderTaskComplete(string result, Exception exception)
        {
            if (IsCanceled) return;
            
            if (exception != null)
            {
                Exception = exception;
                OnComplete?.Invoke(null, exception);
            }
            else
            {
                try
                {
                    var resultObject = Deserialize(result);
                    Result = resultObject;
                    OnComplete?.Invoke(resultObject, null);
                }
                catch (Exception ex)
                {
                    Exception = ex;
                    OnComplete?.Invoke(null, ex);
                }
            }

            IsRunning = false;
            IsDone = true;
            
            OnTaskCoroutineFinished?.Invoke();
        }

        protected abstract T Deserialize(string data);
        
        public void Cancel()
        {
            IsRunning = false;
            IsDone = true;
            IsCanceled = true;
            _stringLoaderTask.Cancel();
        }

        public void Run()
        {
            if (!IsCanceled && !IsDone && !IsRunning)
            {
                IsRunning = true;
                _stringLoaderTask.Run();
            }
        }

        private bool IsCanceled { get; set; }
        public bool IsRunning { get; private set; }
        public bool IsDone { get; private set; }
        public T Result { get; private set; }
        public Exception Exception { get; private set; }
        public event Action<T, Exception> OnComplete;

    }
    
}