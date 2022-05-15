// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using UnityEngine;

#endregion

namespace Amax.Web
{
    public sealed class WebLoader : MonoBehaviour
    {
        
        public static IWebLoader Instance { get; set; }

        private void Awake()
        {
            Instance ??= GetComponent<IWebLoader>();
        }

        public static IWebLoaderTask<T> Load<T>(string url, WWWForm form = null, Action<T, Exception> onComplete = null, EWebLoaderRequestContext context = EWebLoaderRequestContext.Scene) where T : class
            => Instance.Load<T>(url, form, onComplete, context);

        public static IWebLoaderTask<T> LoadFromJson<T>(string url, WWWForm form = null, Action<T, Exception> onComplete = null, EWebLoaderRequestContext context = EWebLoaderRequestContext.Scene) where T : class
            => Instance.LoadFromJson<T>(url, form, onComplete, context);
        
        public static IWebLoaderTask<T> LoadFromXml<T>(string url, WWWForm form = null, Action<T, Exception> onComplete = null, EWebLoaderRequestContext context = EWebLoaderRequestContext.Scene) where T : class
            => Instance.LoadFromXml<T>(url, form, onComplete, context);
        
    }
}