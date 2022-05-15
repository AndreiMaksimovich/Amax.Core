// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

#endregion

namespace Amax.Web
{
   
    internal class WebLoaderTaskDeserializerXml<T> : AWebLoaderTaskDeserializer<T> where T : class
    {

        public WebLoaderTaskDeserializerXml(MonoBehaviour coroutineRunner, IWebLoaderDataProcessorProvider dataProcessorProvider, string url, WWWForm wwwForm, Action<T, Exception> onComplete) : base(coroutineRunner, dataProcessorProvider, url, wwwForm, onComplete) { }

        protected override T Deserialize(string data)
        {
            var serializer = new XmlSerializer(typeof(T));
            return serializer.Deserialize(new StringReader(data)) as T;
        }

    }
    
}