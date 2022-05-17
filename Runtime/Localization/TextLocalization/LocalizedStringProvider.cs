// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Amax.Localization
{
    [CreateAssetMenu(menuName = "Amax/Localization/LocalizedStringsSO", fileName = "LocalizedStrings.asset")]
    public class LocalizedStringProvider: ALocalizedStringProvider
    {
        
        [field: SerializeField] public override string Id { get; set; }
        [field: SerializeField] public string StringIdPrefix { get; set; }
        [field: SerializeField] public List<LocalizedString> LocalizedStrings { get; set; }
        [field: SerializeField] public List<LocalizedStringGroup> LocalizedStringGroups { get; set; }


        public override void GetStrings(Action<string, Dictionary<SystemLanguage, string>> processString)
        {
            foreach (var locString in LocalizedStrings)
            {
                ProcessString(locString, StringIdPrefix, processString);
            }
            foreach (var group in LocalizedStringGroups)
            {
                ProcessGroup(group, StringIdPrefix, processString);
            }
        }

        private void ProcessGroup(LocalizedStringGroup group, string idPrefix, Action<string, Dictionary<SystemLanguage, string>> processString)
        {
            var groupIdPrefix = LocalizedStringIdUtils.Join(idPrefix, group.idPrefix);
            foreach (var locString in group.strings)
            {
                ProcessString(locString, groupIdPrefix, processString);
            }
        }

        private void ProcessString(LocalizedString locString, string idPrefix, Action<string, Dictionary<SystemLanguage, string>> processString)
        {
            processString.Invoke(LocalizedStringIdUtils.Join(idPrefix, locString.id), locString.values);
        }

        
        [Serializable]
        public class LocalizedString
        {
            public string id;
            public LocalizedStringDictionary values;
        }

        [Serializable]
        public class LocalizedStringGroup
        {
            public string idPrefix;
            public List<LocalizedString> strings;
        }
        
    }
}