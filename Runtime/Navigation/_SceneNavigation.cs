// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using System.Collections.Generic;

#endregion

namespace Amax.Navigation
{

    public interface ISceneNavigation
    {
        public List<Scene> History { get; }
        public Scene CurrentScene { get; }
        public Scene HomeScene { get; }
        public Scene MainMenuScene { get; }
        public ISceneController CurrentSceneController { get; }

        public ILoadSceneAsyncTask LoadScene(OpenSceneIntent intent);

        public ILoadSceneAsyncTask LoadScene(Scene scene, bool activateImmediately = true, bool clearHistory = false,
            bool addCurrentSceneToHistory = true);
        public ILoadSceneAsyncTask LoadHomeScene(bool activateImmediately = true);
        public ILoadSceneAsyncTask LoadMainMenuScene(bool activateImmediately = true);
        public ILoadSceneAsyncTask LoadPreviousScene(bool activateImmediately = true);
        public ILoadSceneAsyncTask LoadPreviousScene(int index, bool activateImmediately = true);
    }
    
    public interface ILoadSceneAsyncTask
    {
        public OpenSceneIntent OpenSceneIntent { get; }
        public void ActivateScene();
        public bool IsSceneActivationAllowed { get; set; }
        public bool IsLoading { get; }
        public float Progress { get; }
        public event Action<ILoadSceneAsyncTask> OnSceneLoaded;
        public event Action<ILoadSceneAsyncTask> OnSceneLoadFailed;
        public event Action<ILoadSceneAsyncTask> OnSceneActivation;
    }

    public interface ISceneController
    {
        public void Configure(Scene scene);
        public object CurrentState { get; }
    }

    public class OnSceneControllerAvailable : EventBusBaseEvent
    {
        public ISceneController SceneController { get; }
        public OnSceneControllerAvailable(ISceneController sceneController)
        {
            SceneController = sceneController;
        }
    }

    public class LoadSceneException : Exception
    {
        public LoadSceneException(string message) : base(message) {}
        public LoadSceneException(string message, Exception innerException) : base(message, innerException) {}
    }

}