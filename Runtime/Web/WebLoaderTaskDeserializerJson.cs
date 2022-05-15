// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using UnityEngine;

#endregion

namespace Amax.Web
{
    internal class WebLoaderTaskDeserializerJson<T> : AWebLoaderTaskDeserializer<T> where T : class
    {
        
        public WebLoaderTaskDeserializerJson(MonoBehaviour coroutineRunner, IWebLoaderDataProcessorProvider dataProcessorProvider, string url, WWWForm wwwForm, Action<T, Exception> onComplete) : base(coroutineRunner, dataProcessorProvider, url, wwwForm, onComplete) { }
        
        protected override T Deserialize(string data)
            => JsonUtility.FromJson<T>(data);

    }
    
}