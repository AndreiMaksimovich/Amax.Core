// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Amax.Localization
{
    
    [CreateAssetMenu(menuName = "Amax/Localization/LocalizedStringsCSV", fileName = "LocalizedStringsCsv.asset")]
    public class LocalizedStringProviderCsv: ALocalizedStringProvider
    {
        [field: SerializeField] public override string Id { get; set; }
        [field: SerializeField] public string StringIdPrefix { get; set; }
        [field: SerializeField] public TextAsset CsvFile { get; set; }
        
        [NonSerialized] private bool _isInitialized = false;
        
        private Dictionary<string, Dictionary<SystemLanguage, string>> Strings { get; set; } =
            new Dictionary<string, Dictionary<SystemLanguage, string>>();

        public override void GetStrings(Action<string, Dictionary<SystemLanguage, string>> processString)
        {
            ReadCsv();
            foreach (var id in Strings.Keys)
            {
                processString.Invoke(id, Strings[id]);
            }
        }

       
        
        private void ReadCsv()
        {
            if (_isInitialized) return;
            _isInitialized = true;
            
            Strings.Clear();
            var csv = CsvUtils.Read(CsvFile);
            var languages = new SystemLanguage[csv.HeaderRow.Count];
            for (var i = 1; i < languages.Length; i++)
            {
                languages[i] = LanguageCodes.GetLanguage(csv[0, i].Trim(), SystemLanguage.Unknown);
                #if UNITY_EDITOR
                if (languages[i] == SystemLanguage.Unknown) Debug.LogWarning($"Check languages in {CsvFile.name} -> '{csv[0, i]}'");
                #endif
            }
            for (var row = 1; row < csv.RowCount; row++)
            {
                if (string.IsNullOrEmpty(csv[row, 0])) continue;
                var id = LocalizedStringIdUtils.Join(StringIdPrefix, csv[row, 0]);
                var values = new Dictionary<SystemLanguage, string>();
                for (var column = 1; column < languages.Length; column++)
                {
                    values.Add(languages[column], csv[row, column]);
                    //Debug.Log($"{id}[{languages[column]}] = {csv[row, column]}");
                }
                //Debug.Log($"{id}");
                Strings.Add(id, values);
            }
        }

        private void Dispose()
        {
            IsInitialized = false;
            Strings.Clear();
        }

        private bool IsInitialized { get; set; }
    }
}