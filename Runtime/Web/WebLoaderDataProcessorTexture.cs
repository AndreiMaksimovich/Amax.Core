// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using UnityEngine;
using UnityEngine.Networking;

#endregion

namespace Amax.Web
{
    internal class WebLoaderDataProcessorTexture: MonoBehaviour, IWebLoaderDataProcessor
    {
        
        public bool IsSupported(Type type)
            => type == typeof(Texture) || type == typeof(Texture2D);

        public T ProcessWebRequest<T>(UnityWebRequest webRequest) where T : class
        {
            var texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            texture.LoadImage(webRequest.downloadHandler.data);
            return texture as T;
        }
        
    }
}