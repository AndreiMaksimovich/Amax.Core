// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using UnityEngine;

#endregion

namespace Amax.Navigation
{
    
    public abstract class ASceneController: MonoBehaviour, ISceneController
    {
        public virtual void Configure(Scene scene) { }

        public virtual object CurrentState => null;
        
        protected virtual void Awake()
        {
            EventBus.RaiseEvent(new OnSceneControllerAvailable(this));
        }
        
    }
    
}