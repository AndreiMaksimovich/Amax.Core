// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using UnityEngine;
using UnityEngine.Networking;

#endregion

namespace Amax.Web
{
    internal class WebLoaderDataProcessorByteArray: MonoBehaviour, IWebLoaderDataProcessor
    {
        
        public bool IsSupported(Type type)
            => type == typeof(byte[]);
        
        public T ProcessWebRequest<T>(UnityWebRequest webRequest) where T : class
            => webRequest.downloadHandler.data as T;
        
    }
}
