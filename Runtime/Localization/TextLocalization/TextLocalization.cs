// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using UnityEngine;

#endregion

namespace Amax.Localization
{
    
    [DefaultExecutionOrder(-100)]
    public class TextLocalization: MonoBehaviour
    {
        
        public static ITextLocalization Instance { get; set; }
        
        private void Awake()
        {
            Instance ??= GetComponent<ITextLocalization>();
        }

        public static string Get(string id, SystemLanguage language = SystemLanguage.Unknown, SystemLanguage defaultLanguage = SystemLanguage.Unknown, string defaultValue = null)
            => Instance.Get(id, language, defaultLanguage, defaultValue);

        public static string Get(params string[] id) => Instance.Get(id);
        public static string Get(SystemLanguage language, params string[] id) => Instance.Get(language, id);
        public static string Get(SystemLanguage language, SystemLanguage defaultLanguage, params string[] id) => Instance.Get(language, defaultLanguage, id);
        
        public static SystemLanguage Language 
        { 
            get => Instance.Language;
            set => Instance.Language = value;
        }
        
        public static SystemLanguage DefaultLanguage
        { 
            get => Instance.DefaultLanguage;
            set => Instance.DefaultLanguage = value;
        }
        
    }
}