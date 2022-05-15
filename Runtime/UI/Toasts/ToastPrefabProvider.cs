// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Amax.UI.Toasts
{
    public class ToastPrefabProvider: MonoBehaviour, IToastPrefabProvider
    {

        [Header("Prefabs")]
        [SerializeField] private GameObject defaultPrefab;
        [SerializeField] private GameObject infoPrefab, warningPrefab, errorPrefab;

        public List<ICustomToastPrefabProvider> CustomToastPrefabProviders { get; set; } =
            new List<ICustomToastPrefabProvider>();

        private void Awake()
        {
            foreach (var customToastPrefabProvider in GetComponentsInChildren<ICustomToastPrefabProvider>())
            {
                CustomToastPrefabProviders.Add(customToastPrefabProvider);
            }
        }

        public GameObject GetPrefab(IToastContent content)
        {
            foreach (var customToastPrefabProvider in CustomToastPrefabProviders)
            {
                if (customToastPrefabProvider.IsSupported(content)) return customToastPrefabProvider.GetPrefab(content);
            }

            return content.Style switch
            {
                EToastStyle.Info => infoPrefab,
                EToastStyle.Warning => warningPrefab,
                EToastStyle.Error => errorPrefab,
                _ => defaultPrefab
            };
        }
        
    }
}