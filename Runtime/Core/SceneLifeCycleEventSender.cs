// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using UnityEngine;

#endregion

namespace Amax { 

    [DefaultExecutionOrder(100)]
    public class SceneLifeCycleEventSender: MonoBehaviour
    {

        private void Start()
        {
            EventBus.RaiseEvent(new OnSceneStart());
        }

        private void OnDestroy()
        {
            EventBus.RaiseEvent(new OnSceneDestroy());
        }

        private void Awake()
        {
            EventBus.RaiseEvent(new OnSceneAwake());
        }
        
    }

}
