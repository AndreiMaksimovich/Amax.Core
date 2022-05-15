// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

#endregion

namespace Amax.Localization
{
    
    [DefaultExecutionOrder(-100)]
    public class TextLocalizationImplementation: MonoBehaviour, ITextLocalization, IEventBusListener<OnSceneDestroy>, IEventBusListener<OnLocalizedStringProviderInstallerAvailable>
    {

        private const string LogTag = "TextLocalization:";
        
        [field: SerializeField] private TextLocalizationConfiguration Configuration { get; set; }

        private Dictionary<string, Dictionary<SystemLanguage, string>> Strings { get; } = new Dictionary<string, Dictionary<SystemLanguage, string>>();
        private Dictionary<string, LocalizedStringProviderReferenceCounter> ProviderRefCounters { get; } = new Dictionary<string, LocalizedStringProviderReferenceCounter>();

        public string Get(string id, SystemLanguage language = SystemLanguage.Unknown, SystemLanguage defaultLanguage = SystemLanguage.Unknown, string defaultValue = null)
        {

            if (language == SystemLanguage.Unknown) language = Language;
            if (defaultLanguage == SystemLanguage.Unknown) defaultLanguage = DefaultLanguage;
            
            if (!Strings.ContainsKey(id))
            {
#if UNITY_EDITOR
                Debug.LogWarning($"{LogTag} string with id={id} not found");
                return $"String id={id} not found";
#endif
                return defaultValue;
            }
            var values = Strings[id];
            if (!values.ContainsKey(language))
            {
#if UNITY_EDITOR
                Debug.LogWarning($"{LogTag} string with id={id} has no localization for language={language}");
#endif
                return values.ContainsKey(defaultLanguage) ? values[defaultLanguage] : defaultValue;
            }
            return values[language];
        }

        public string Get(params string[] id)
            => Get(LocalizedStringIdUtils.Join(id), Language, DefaultLanguage);

        public string Get(SystemLanguage language, params string[] id)
            => Get(LocalizedStringIdUtils.Join(id), language, DefaultLanguage);

        public string Get(SystemLanguage language, SystemLanguage defaultLanguage, params string[] id)
            => Get(LocalizedStringIdUtils.Join(id), language, defaultLanguage);

        private SystemLanguage _language = SystemLanguage.Unknown, _defaultLanguage = SystemLanguage.Unknown;
        public SystemLanguage Language
        {
            get
            {
                if (_language == SystemLanguage.Unknown)
                {
                    _language = GetLanguageSetting(ELanguageSettingType.Language);
                }
                return _language;
            }
            set
            {
                _language = value;
                SaveLanguageSetting(ELanguageSettingType.Language, value);
                EventBus.RaiseEvent(new OnTextLocalizationLanguageChanged());
            }
        }
        
        public SystemLanguage DefaultLanguage
        {
            get
            {
                if (_defaultLanguage == SystemLanguage.Unknown)
                {
                    _defaultLanguage = GetLanguageSetting(ELanguageSettingType.DefaultLanguage);
                }
                return _defaultLanguage;
            }
            set
            {
                _defaultLanguage = value;
                SaveLanguageSetting(ELanguageSettingType.DefaultLanguage, value);
                EventBus.RaiseEvent(new OnTextLocalizationLanguageChanged());
            }
        }
        
        private SystemLanguage GetLanguageSetting(ELanguageSettingType type)
            => 
                Enum.TryParse<SystemLanguage>
                (
                    PlayerPrefs.GetString(GetPlayerPreferencesLanguageSettingKey(type), null), 
                    out var language
                ) ? language : Configuration.DefaultLanguage;
        

        private void SaveLanguageSetting(ELanguageSettingType type, SystemLanguage language)
        {
            PlayerPrefs.SetString(GetPlayerPreferencesLanguageSettingKey(type), language.ToString());
        }

        private enum ELanguageSettingType
        {
            Language,
            DefaultLanguage
        }

        private string GetPlayerPreferencesLanguageSettingKey(ELanguageSettingType type)
            => $"Amax.Localization.TextLocalization.Settings.{type.ToString()}";
        
        private void Awake()
        {
            EventBus.AddListener(this as IEventBusListener<OnSceneDestroy>);
            EventBus.AddListener(this as IEventBusListener<OnLocalizedStringProviderInstallerAvailable>);
            if (TextLocalization.Instance == null) Initialize();
        }

        private void OnDestroy()
        {
            EventBus.RemoveListener(this as IEventBusListener<OnSceneDestroy>);
            EventBus.RemoveListener(this as IEventBusListener<OnLocalizedStringProviderInstallerAvailable>);
        }

        private bool _isInitialized = false;
        
        private void Initialize()
        {
            if (_isInitialized) return;
            _isInitialized = true;
            if (Configuration == null) return;
            foreach (var lsp in Configuration.GlobalLocalizedStringProviders)
            {
                Install(lsp, ETextLocalizationContext.Global);
            }
#if UNITY_EDITOR
            //PrintDebugStrings();
#endif
        }

        public void OnEvent(OnLocalizedStringProviderInstallerAvailable data)
        {
            foreach (var lsp in data.Installer.LocalizedStringProviders)
            {
                Install(lsp, data.Installer.LocalizationContext);
            }
            if (data.Installer.DisposeProvidersOnInstallerDestroy) data.Installer.OnInstallerDestroy += DisposeInstaller;
        }
        
        public void OnEvent(OnSceneDestroy data)
        {
            var lspRefCountersToRemove = new List<LocalizedStringProviderReferenceCounter>();
            
            foreach (var lspId in ProviderRefCounters.Keys)
            {
                var lspRefCounter = ProviderRefCounters[lspId];
                lspRefCounter.SceneRefCount = 0;
                if (lspRefCounter.GlobalRefCount == 0) lspRefCountersToRemove.Add(lspRefCounter);
            }
            
            if (lspRefCountersToRemove.Count < 1) return;
            foreach (var lspRefCounterToRemove in lspRefCountersToRemove)
            {
                Remove(lspRefCounterToRemove);
            }
        }

        private void DisposeInstaller(ILocalizedStringProviderInstaller installer)
        {
            foreach (var lsp in installer.LocalizedStringProviders)
            {
                Remove(lsp, installer.LocalizationContext);
            }
        }

        private void Remove(ILocalizedStringProvider provider, ETextLocalizationContext context)
        {
            if (!ProviderRefCounters.ContainsKey(provider.Id)) return;
            var lspRefCount = ProviderRefCounters[provider.Id];
            if (context == ETextLocalizationContext.Global)
            {
                lspRefCount.GlobalRefCount--;
            }
            else
            {
                lspRefCount.SceneRefCount--;
            }
            if (lspRefCount.GlobalRefCount == 0 && lspRefCount.SceneRefCount == 0)
            {
                Remove(lspRefCount);
            }
        }

        private void Remove(LocalizedStringProviderReferenceCounter lspRefCount)
        {
            foreach (var stringId in lspRefCount.StringIds)
            {
                Strings.Remove(stringId);
            }
            ProviderRefCounters.Remove(lspRefCount.Id);
        }

        private void Install(ILocalizedStringProvider provider, ETextLocalizationContext context)
        {
            LocalizedStringProviderReferenceCounter lspReference;
            
            // New
            if (!ProviderRefCounters.ContainsKey(provider.Id))
            {
                lspReference = new LocalizedStringProviderReferenceCounter()
                {
                    Id = provider.Id
                };
                ProviderRefCounters.Add(provider.Id, lspReference);
                
                provider.GetStrings(
                    (stringId, values) =>
                    {
                        lspReference.StringIds.Add(stringId);
                        Strings.Add(stringId, values);
                    }
                );
            }
            // Exists
            else
            {
                lspReference = ProviderRefCounters[provider.Id];
            }
            
            if (context == ETextLocalizationContext.Global)
            {
                lspReference.GlobalRefCount++;
            }
            else
            {
                lspReference.SceneRefCount++;
            }
            
        } 

        private class LocalizedStringProviderReferenceCounter
        {
            public string Id { get; set; }
            public List<string> StringIds { get; set; } = new List<string>();
            public int GlobalRefCount { get; set; }
            public int SceneRefCount { get; set; }
        }
        
#if UNITY_EDITOR

        public void PrintDebugStrings()
        {
            var stringBuilder = new StringBuilder();
            
            foreach (var keyValuePair in Strings)
            {
                stringBuilder.Append($"- - -> {keyValuePair.Key}\n");
                foreach (var langToValue in keyValuePair.Value)
                {
                    stringBuilder.Append($"{langToValue.Key} = \"{langToValue.Value}\"\n");
                }
            }
            
            Debug.Log(stringBuilder.ToString());
            
        }
        
#endif
        
        
    }
    
}