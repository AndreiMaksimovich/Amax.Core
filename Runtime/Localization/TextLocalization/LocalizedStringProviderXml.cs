// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

#endregion

namespace Amax.Localization
{
    [CreateAssetMenu(menuName = "Amax/Localization/LocalizedStringsXML", fileName = "LocalizedStringsXml.asset")]
    public class LocalizedStringProviderXml: ALocalizedStringProvider
    {

        [field: SerializeField] public override string Id { get; set; }
        [field: SerializeField] public TextAsset XmlFile;
        private Dictionary<string, Dictionary<SystemLanguage, string>> Strings { get; } = new Dictionary<string, Dictionary<SystemLanguage, string>>();

        [NonSerialized] private LocalizedStringsXmlFile _localization;
        [NonSerialized] private bool _isInitialized = false;
        
        public override void GetStrings(Action<string, Dictionary<SystemLanguage, string>> processString)
        {
            ReadXmlFile();
            foreach (var id in Strings.Keys)
            {
                processString.Invoke(id, Strings[id]);
            }
        }
        
        private void ReadXmlFile()
        {
            if (_isInitialized) return;
            _isInitialized = true;
            
            var xmlSerializer = new XmlSerializer(typeof(LocalizedStringsXmlFile));
            var stringReader = new StringReader(XmlFile.text);
            _localization = xmlSerializer.Deserialize(stringReader) as LocalizedStringsXmlFile;
            
            // strings
            if (_localization.strings!=null) foreach (var locString in _localization.strings)
            {
                ReadXmlProcessString(locString, _localization.idPrefix);
            }
            
            // groups
            if (_localization.groups!=null) foreach (var group in _localization.groups)
            {
                ReadXmlProcessGroup(group, _localization.idPrefix);
            }
            
        }

        private void ReadXmlProcessString(LocalizedStringsXmlFileString locStr, string idPrefix)
        {
            var id = LocalizedStringIdUtils.Join(idPrefix, locStr.id);
            var strings = new Dictionary<SystemLanguage, string>();
            foreach (var languageCode in locStr.values.Keys)
            {
                var language = LanguageCodes.GetLanguage(languageCode, SystemLanguage.Unknown);
#if UNITY_EDITOR
                if (language == SystemLanguage.Unknown) Debug.LogWarning($"Text localization, Xml file {XmlFile.name}, string {locStr.id}, Check language codes");
#endif
                strings[language] = locStr.values[languageCode];
            }
            Strings.Add(id, strings);
        }

        private void ReadXmlProcessGroup(LocalizedStringsXmlFileStringGroup group, string idPrefix = null)
        {
            var groupIdPrefix = LocalizedStringIdUtils.Join(idPrefix, group.idPrefix);
            // strings
            if (group.strings != null) 
                foreach (var locString in group.strings)
                {
                    ReadXmlProcessString(locString, groupIdPrefix);
                }
            // groups
            if (group.groups != null) 
                foreach (var innerGroup in group.groups)
                {
                    ReadXmlProcessGroup(innerGroup, groupIdPrefix);
                }
        }
        
    }
}