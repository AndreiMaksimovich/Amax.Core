// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using UnityEngine;

#endregion

namespace Amax 
{

    [DefaultExecutionOrder(-10000)]
    public class Singleton<T> : MonoBehaviour where T : Component 
    {
        
        private static T _instance;

        public static T Instance 
        {
            get => _instance;
            set => _instance = value;
        }

        private bool IsSameType => GetType() == typeof(T);

        private void Awake() 
        {
            InitializeInstance();
		}

        protected virtual void OnAwake() { }

        private void InitializeInstance()
        {
            if (_instance == null) 
            {
                if (IsSameType) {
                    _instance = this as T;
                    OnAwake();
                }
                else
                {
                    var existingInstances = FindObjectsOfType<T>();
                    if (existingInstances == null || existingInstances.Length == 0)
                    {
                        _instance = gameObject.AddComponent<T>();
                    }
                    else
                    {
                        _instance = existingInstances[0];
                        for (int i = 1; i < existingInstances.Length; i++)
                        {
#if UNITY_EDITOR
                            DestroyImmediate(existingInstances[i]);
#else
                            DestroyImmediate(existingInstances[i]);
#endif
                        }
                    }
                }
            }
            else 
            {
                if (IsSameType)
                {

                    if (_instance != this)
                    {
#if UNITY_EDITOR
                        if (Application.isPlaying)  DestroyImmediate(this);
#else
                        DestroyImmediate(this);
#endif
                    }
                }
            }
        }
    
	}

}
