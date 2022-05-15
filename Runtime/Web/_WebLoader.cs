// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using UnityEngine;
using UnityEngine.Networking;

#endregion

namespace Amax.Web
{
    
    public interface IWebLoader
    {
        IWebLoaderTask<T> Load<T>(string url, WWWForm form = null, Action<T, Exception> onComplete = null, EWebLoaderRequestContext context = EWebLoaderRequestContext.Scene) where T : class;
        IWebLoaderTask<T> LoadFromJson<T>(string url, WWWForm form = null, Action<T, Exception> onComplete = null, EWebLoaderRequestContext context = EWebLoaderRequestContext.Scene) where T : class;
        IWebLoaderTask<T> LoadFromXml<T>(string url, WWWForm form = null, Action<T, Exception> onComplete = null, EWebLoaderRequestContext context = EWebLoaderRequestContext.Scene) where T : class;
        
    }
    
    public enum EWebLoaderRequestContext
    {
        Scene,
        Global
    }

    public interface IWebLoaderTaskBase
    {
        void Cancel();
        bool IsDone { get; }
    }
    
    public interface IWebLoaderTask<out T>: IWebLoaderTaskBase  where T : class
    {
        T Result { get; }
        Exception Exception { get; }
        event Action<T, Exception> OnComplete;
    }

    public interface IWebLoaderDataProcessorProvider
    {
        public IWebLoaderDataProcessor GetDataProcessor<T>() where T : class;
    }

    
    public interface IWebLoaderDataProcessor
    {
        public bool IsSupported(Type type);
        public T ProcessWebRequest<T>(UnityWebRequest webRequest) where T : class;
    }
    
    public class WebLoaderConnectionError : Exception {}
    public class WebLoaderProtocolError : Exception {}

    public class WebLoaderDataProcessingError : Exception
    {
        public WebLoaderDataProcessingError(): base() {}
        public WebLoaderDataProcessingError(string message, Exception ex): base(message, ex) {}
    }
    
}


