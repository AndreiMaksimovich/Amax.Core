// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

namespace Amax.Navigation
{
    public class SceneNavigationImplementation: MonoBehaviour, ISceneNavigation, IEventBusListener<OnSceneDestroy>, IEventBusListener<OnSceneAwake>, IEventBusListener<OnSceneStart>, IEventBusListener<OnSceneControllerAvailable>
    {
        
        private const string LogTag = "SceneNavigation:";
        
        [field: SerializeField] private SceneNavigationConfiguration Configuration { get; set; }
        
        public List<Scene> History { get; } = new ();
        public Scene CurrentScene { get; private set; }
        public Scene HomeScene { get; private set; }
        public Scene MainMenuScene { get; private set; }
        public ISceneController CurrentSceneController { get; private set; }
        
        private OpenSceneIntent NextSceneIntent { get; set; }

        private ILoadSceneAsyncTask LoadScene(OpenSceneIntent intent, Action beforeSceneActivation)
            => new LoadSceneAsyncTask
                (this, intent, () =>
                    {
                        beforeSceneActivation?.Invoke();
                        NextSceneIntent = intent;
                    }
                );
        
        public ILoadSceneAsyncTask LoadScene(OpenSceneIntent intent)
            => LoadScene
                (
                    intent, () =>
                    {
                        if (intent.ClearHistory)
                        {
                            History.Clear();
                        }
                        if (intent.AddCurrentSceneToHistory)
                        {
                            if (CurrentSceneController != null)
                                CurrentScene.SavedState = CurrentSceneController.CurrentState;
                            History.Add(CurrentScene);
                        }
                    }
                );
        

        public ILoadSceneAsyncTask LoadScene(Scene scene, bool activateImmediately = true, bool clearHistory = false, bool addCurrentSceneToHistory = true)
            => LoadScene
                (
                    new OpenSceneIntent()
                    {
                        Scene = scene,
                        ClearHistory = clearHistory,
                        AddCurrentSceneToHistory = addCurrentSceneToHistory,
                        ActivateSceneImmediately = activateImmediately
                    }
                );

        public ILoadSceneAsyncTask LoadMainMenuScene(bool activateImmediately = true)
            => LoadScene(HomeScene, activateImmediately, true, false);

        public ILoadSceneAsyncTask LoadHomeScene(bool activateImmediately = true)
            => LoadScene
            (
                new OpenSceneIntent()
                {
                    Scene = HomeScene,
                    AddCurrentSceneToHistory = false,
                    ClearHistory = true,
                    ActivateSceneImmediately = activateImmediately
                }
            );
        
        public ILoadSceneAsyncTask LoadPreviousScene(bool activateImmediately = true)
        {
            if (History.Count == 0)
            {
                var message = $"{LogTag} LoadPreviousScene History Is Empty";
                Debug.LogWarning(message);
                throw new LoadSceneException(message);
            }
            return LoadPreviousScene(History.Count - 1, activateImmediately);
        }

        public ILoadSceneAsyncTask LoadPreviousScene(int index, bool activateImmediately = true)
        {
            if (History.Count <= index)
            {
                var message = $"{LogTag} LoadPreviousScene  History does not contain index={index}";
                Debug.LogWarning(message);
                throw new LoadSceneException(message);
            }
            return LoadScene
            (
                new OpenSceneIntent()
                {
                    Scene = History[History.Count - 1],
                    ClearHistory = false,
                    AddCurrentSceneToHistory = false,
                    ActivateSceneImmediately = activateImmediately
                },
                () =>
                {
                    for (var i = History.Count - 1; i >= index; i--)
                    {
                        History.RemoveAt(i);
                    }
                }
            );
        }
        
        private void Awake()
        {
            EventBus.AddListener(this as IEventBusListener<OnSceneAwake>);
            EventBus.AddListener(this as IEventBusListener<OnSceneStart>);
            EventBus.AddListener(this as IEventBusListener<OnSceneDestroy>);
            EventBus.AddListener(this as IEventBusListener<OnSceneControllerAvailable>);
            HomeScene = Configuration.HomeScene.ToScene();
            MainMenuScene = Configuration.MainMenuScene.ToScene();
        }

        private void OnDestroy()
        {
            EventBus.RemoveListener(this as IEventBusListener<OnSceneAwake>);
            EventBus.RemoveListener(this as IEventBusListener<OnSceneStart>);
            EventBus.RemoveListener(this as IEventBusListener<OnSceneDestroy>);
            EventBus.RemoveListener(this as IEventBusListener<OnSceneControllerAvailable>);
        }
        
        public void OnEvent(OnSceneStart data)
        {
            CurrentSceneController?.Configure(CurrentScene);
        }
        
        public void OnEvent(OnSceneDestroy data)
        {
            CurrentSceneController = null;
        }

        public void OnEvent(OnSceneControllerAvailable data)
        {
            CurrentSceneController = data.SceneController;
        }

        public void OnEvent(OnSceneAwake data)
        {
            if (NextSceneIntent != null)
            {
                CurrentScene = NextSceneIntent.Scene;
            }
            else
            {
                CurrentScene = new Scene(SceneManager.GetActiveScene().buildIndex);
            }
            NextSceneIntent = null;
        }
        
    }
}