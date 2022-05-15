// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Amax.UI.Dialogs
{
    public class DialogPrefabProvider: MonoBehaviour, IDialogPrefabProvider
    {
        
        [field: SerializeField] public GameObject DefaultDialogPrefab { get; set; }
        [field: SerializeField] public List<DialogPrefabToStyleAndSize> DialogPrefabs { get; set; }

        public List<ICustomDialogPrefabProvider> CustomDialogPrefabProviders { get; set; } =
            new List<ICustomDialogPrefabProvider>();

        private void Awake()
        {
            foreach (var customDialogPrefabProvider in GetComponentsInChildren<ICustomDialogPrefabProvider>())
            {
                CustomDialogPrefabProviders.Add(customDialogPrefabProvider);
            }
        }

        public GameObject GetPrefab(IDialogContent content)
        {
            // Custom providers
            foreach (var customProvider in CustomDialogPrefabProviders)
            {
                if (customProvider.IsSupported(content))
                {
                    return customProvider.GetPrefab(content);
                }
            }
            // Mapping
            foreach (var provider in DialogPrefabs)
            {
                if (
                    (provider.everySize || provider.size == content.Size) 
                    &&
                    (provider.everyStyle || provider.style == content.Style)
                )
                {
                    return provider.prefab;
                }
            }
            return DefaultDialogPrefab;
        }

        [Serializable]
        public class DialogPrefabToStyleAndSize
        {
            public bool everyStyle;
            public EDialogStyle style;
            public bool everySize;
            public EDialogSize size;
            public GameObject prefab;
        }
        
    }
}