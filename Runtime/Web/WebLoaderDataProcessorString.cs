// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using UnityEngine;
using UnityEngine.Networking;

#endregion

namespace Amax.Web
{
    internal class WebLoaderDataProcessorString: MonoBehaviour, IWebLoaderDataProcessor
    {
        
        public bool IsSupported(Type type)
            => type == typeof(string);

        public T ProcessWebRequest<T>(UnityWebRequest webRequest) where T : class
            => new string(webRequest.downloadHandler.text) as T;
        
    }
}