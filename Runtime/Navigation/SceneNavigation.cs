// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Amax.Navigation
{
    public class SceneNavigation: MonoBehaviour
    {
        
        public static ISceneNavigation Instance { get; set; }

        private void Awake()
        {
            Instance ??= GetComponent<ISceneNavigation>();
        }

        public static List<Scene> History => Instance.History;
        public static Scene CurrentScene => Instance.CurrentScene;
        public static ISceneController CurrentSceneController => Instance.CurrentSceneController;
        public static Scene HomeScene => Instance.HomeScene;


        public static ILoadSceneAsyncTask LoadScene(OpenSceneIntent intent)
            => Instance.LoadScene(intent);
        
        public static ILoadSceneAsyncTask LoadHomeScene(bool activateImmediately = true)
            => Instance.LoadHomeScene(activateImmediately);
        
        public static ILoadSceneAsyncTask LoadPreviousScene(bool activateImmediately = true)
            => Instance.LoadPreviousScene(activateImmediately);

        public static ILoadSceneAsyncTask LoadPreviousScene(int index, bool activateImmediately = true)
            => Instance.LoadPreviousScene(index, activateImmediately);

        public static ILoadSceneAsyncTask LoadMainMenuScene(bool activateImmediately = true)
            => Instance.LoadMainMenuScene(activateImmediately);

        public static ILoadSceneAsyncTask LoadScene(Scene scene, bool activateImmediately = true, bool clearHistory = false, bool addCurrentSceneToHistory = true)
            => Instance.LoadScene(scene, activateImmediately, clearHistory, addCurrentSceneToHistory);

    }
}