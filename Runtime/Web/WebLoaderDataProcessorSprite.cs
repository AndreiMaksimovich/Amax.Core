// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using UnityEngine;
using UnityEngine.Networking;

#endregion

namespace Amax.Web
{
    internal class WebLoaderDataProcessorSprite: MonoBehaviour, IWebLoaderDataProcessor
    {
        
        public bool IsSupported(Type type)
            => type == typeof(Sprite);

        public T ProcessWebRequest<T>(UnityWebRequest webRequest) where T : class
        {
            var texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            texture.LoadImage(webRequest.downloadHandler.data);
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)) as T;
        }
        
    }
}