// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Amax {

    [DefaultExecutionOrder(-10000)]
    public class PersistentGameObjectsRoot : MonoBehaviour
    {

        [SerializeField] private string id;
        private static Dictionary<string, PersistentGameObjectsRoot> Instances { get; } = new ();

        private void Awake()
        {
            if (Instances.ContainsKey(id))
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                Instances.Add(id, this);
                DontDestroyOnLoad(gameObject);
            }
        }
        
    }

}
