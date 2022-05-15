// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

#endregion

namespace Amax.Web
{
    internal class WebLoaderTask<T> : IWebLoaderTask<T> where T : class
    {

            private bool _isCanceled;
            private readonly string _url;
            private readonly WWWForm _wwwForm;
            private UnityWebRequest _webRequest;
            private readonly MonoBehaviour _coroutineRunner;
            private readonly IWebLoaderDataProcessorProvider _dataProcessorProvider;
            private IWebLoaderTask<T> _webLoaderTaskImplementation;
            public event Action OnTaskCoroutineFinished;

            public WebLoaderTask(MonoBehaviour coroutineRunner, IWebLoaderDataProcessorProvider dataProcessorProvider, string url, WWWForm wwwForm, Action<T, Exception> onComplete)
            {
                _url = url;
                _wwwForm = wwwForm;
                if (onComplete!=null) OnComplete += onComplete;
                _coroutineRunner = coroutineRunner;
                _dataProcessorProvider = dataProcessorProvider;
            }
            
            public void Cancel()
            {
                _isCanceled = true;
            }

            public void Run()
            {
                if (IsRunning || IsDone) return;
                IsRunning = true;
                _coroutineRunner.StartCoroutine(TaskCoroutine());
            }

            private IEnumerator TaskCoroutine()
            {
                
                _webRequest = (_wwwForm != null) ? UnityWebRequest.Post(_url, _wwwForm) : new UnityWebRequest(_url);
                _webRequest.downloadHandler = new DownloadHandlerBuffer();

                var asyncOperation = _webRequest.SendWebRequest();
                
                while (!asyncOperation.isDone)
                {
                    if (_isCanceled)
                    {
                        _webRequest.Abort();
                    }
                    else
                    {
                        yield return null;
                    }
                }

                if (!_isCanceled)
                {
                    // webRequest.result == Error
                    if (_webRequest.result != UnityWebRequest.Result.Success)
                    {
                        OnComplete?.Invoke(null, GetException(_webRequest.result));
                    }
                    // webRequest.result == Success
                    else
                    {
                        try
                        {
                            InvokeOnComplete(_dataProcessorProvider.GetDataProcessor<T>().ProcessWebRequest<T>(_webRequest), null);
                        }
                        catch (Exception ex)
                        {
                            InvokeOnComplete(null, new WebLoaderDataProcessingError(ex.Message, ex));
                        }
                    }
                }

                // Finalization
                _webRequest?.downloadHandler.Dispose();
                _webRequest?.Dispose();
                IsDone = true;
                IsRunning = false;
                
                OnTaskCoroutineFinished?.Invoke();
                
            }

            private void InvokeOnComplete(T result, Exception exception)
            {
                Result = result;
                Exception = exception;
                if (!_isCanceled) OnComplete?.Invoke(result, exception);
            }

            public bool IsRunning { get; private set; }
            public bool IsDone { get; private set; }
            public T Result { get; private set; }
            public Exception Exception { get; private set; }
            public event Action<T, Exception> OnComplete;

            private Exception GetException(UnityWebRequest.Result result)
                => result switch
                {
                    UnityWebRequest.Result.ConnectionError => new WebLoaderConnectionError(),
                    UnityWebRequest.Result.ProtocolError => new WebLoaderProtocolError(),
                    UnityWebRequest.Result.DataProcessingError => new WebLoaderDataProcessingError(),
                    _ => new Exception()
                };

        }
}