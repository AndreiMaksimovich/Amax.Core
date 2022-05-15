// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Amax.Web
{
    internal class WebLoaderImplementation: MonoBehaviour, IWebLoader, IWebLoaderDataProcessorProvider, IEventBusListener<OnSceneDestroy>
    {
        
        private List<IWebLoaderDataProcessor> DataProcessors { get; } = new List<IWebLoaderDataProcessor>();

        private readonly List<IWebLoaderTaskBase> _localTasks = new();

        public IWebLoaderTask<T> Load<T>(string url, WWWForm form = null, Action<T, Exception> onComplete = null, EWebLoaderRequestContext context = EWebLoaderRequestContext.Scene) where T : class
        {
            var task = new WebLoaderTask<T>(this, this, url, form, onComplete);
            if (context == EWebLoaderRequestContext.Scene)
            {
                _localTasks.Add(task);
                task.OnTaskCoroutineFinished += () => _localTasks.Remove(task);
            }
            task.Run();
            return task;
        }

        public IWebLoaderTask<T> LoadFromJson<T>(string url, WWWForm form = null, Action<T, Exception> onComplete = null, EWebLoaderRequestContext context = EWebLoaderRequestContext.Scene) where T : class
        {
            var task = new WebLoaderTaskDeserializerJson<T>(this, this, url, form, onComplete);
            if (context == EWebLoaderRequestContext.Scene)
            {
                _localTasks.Add(task);
                task.OnTaskCoroutineFinished += () => _localTasks.Remove(task);
            }
            task.Run();
            return task;
        }
        
        public IWebLoaderTask<T> LoadFromXml<T>(string url, WWWForm form = null, Action<T, Exception> onComplete = null, EWebLoaderRequestContext context = EWebLoaderRequestContext.Scene) where T : class
        {
            var task = new WebLoaderTaskDeserializerXml<T>(this, this, url, form, onComplete);
            if (context == EWebLoaderRequestContext.Scene)
            {
                _localTasks.Add(task);
                task.OnTaskCoroutineFinished += () => _localTasks.Remove(task);
            }
            task.Run();
            return task;
        }

        private void Awake()
        {
            EventBus.AddListener(this);
            foreach (var dataProcessor in GetComponentsInChildren<IWebLoaderDataProcessor>())
            {
                DataProcessors.Add(dataProcessor);
            }
        }

        private void OnDestroy()
        {
            EventBus.RemoveListener(this);
        }

        public IWebLoaderDataProcessor GetDataProcessor<T>() where T : class
        {
            var type = typeof(T);
            foreach (var dataProcessor in DataProcessors)
            {
                if (dataProcessor.IsSupported(type))
                {
                    return dataProcessor;
                }
            }
            return null;
        }

        public void OnEvent(OnSceneDestroy data)
        {
            foreach (var localTask in _localTasks)
            {
                localTask.Cancel();
            }
        }
    }
}