// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using UnityEngine;

#endregion

namespace Amax.Localization
{
    [DefaultExecutionOrder(-50)]
    public class LocalizedStringProviderInstaller: MonoBehaviour, ILocalizedStringProviderInstaller
    {
        [field: SerializeField] public ETextLocalizationContext LocalizationContext { get; set; } = ETextLocalizationContext.Scene;
        [field: SerializeField] public bool DisposeProvidersOnInstallerDestroy { get; set; } = true;
        
        [SerializeField] public ALocalizedStringProvider[] localizedStringProviders;
        public ILocalizedStringProvider[] LocalizedStringProviders => localizedStringProviders;
        
        public event Action<ILocalizedStringProviderInstaller> OnInstallerDestroy;

        private void Awake()
        {
            EventBus.RaiseEvent(new OnLocalizedStringProviderInstallerAvailable(this));
        }

        private void OnDestroy()
        {
            OnInstallerDestroy?.Invoke(this);
        }
    }
}