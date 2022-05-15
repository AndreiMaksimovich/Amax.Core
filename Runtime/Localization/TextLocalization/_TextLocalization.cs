// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Amax.Localization
{

    public interface ITextLocalization
    {
        public string Get(string id, SystemLanguage language = SystemLanguage.Unknown, SystemLanguage defaultLanguage = SystemLanguage.Unknown, string defaultValue = null);
        public string Get(params string[] id);
        public string Get(SystemLanguage language, params string[] id);
        public string Get(SystemLanguage language, SystemLanguage defaultLanguage, params string[] id);
        
        public SystemLanguage Language { get; set; }
        public SystemLanguage DefaultLanguage { get; set; }
    }

    public class OnTextLocalizationLanguageChanged : EventBusBaseEvent { }

    public class OnLocalizedStringProviderInstallerAvailable : EventBusBaseEvent
    {
        public ILocalizedStringProviderInstaller Installer { get; }
        public OnLocalizedStringProviderInstallerAvailable(ILocalizedStringProviderInstaller installer)
        {
            Installer = installer;
        }
    }
    
    public interface ILocalizedStringProvider
    {
        public string Id { get; }
        public void GetStrings(Action<string, Dictionary<SystemLanguage, string>> processString);
    }

    public interface ILocalizedStringProviderInstaller
    {
        public ETextLocalizationContext LocalizationContext { get; }
        public ILocalizedStringProvider[] LocalizedStringProviders { get; }
        public bool DisposeProvidersOnInstallerDestroy { get; }
        public event Action<ILocalizedStringProviderInstaller> OnInstallerDestroy;
    }

    public enum ETextLocalizationContext
    {
        Global,
        Scene
    }

    public interface ITextLocalizationArea
    {
        public bool IsRoot { get; }
        public string StringIdPrefix { get; }
    }

    public interface ITextLocalizer
    {
        public string StringId { get; set; }
        public void Refresh();
    }
    
}