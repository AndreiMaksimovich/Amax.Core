// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

#endregion

namespace Amax.Localization
{

    public static class LanguageCodes 
    {
	    
	    private const SystemLanguage DefaultLanguage = SystemLanguage.English;

		public const string 
			English = "EN",
			Russian = "RU",
			Polish = "PL",
			Belarusian = "BE",
			German = "DE";

		private static readonly Dictionary<SystemLanguage, string> LanguageToLanguageCode = new Dictionary<SystemLanguage, string>();
		private static readonly Dictionary<string, SystemLanguage> LanguageCodeToLanguage = new Dictionary<string, SystemLanguage>();
		
        // ----------------------------------------------------------------
		
		static LanguageCodes() {
            
			Object[] codes = 
			{

				SystemLanguage.English, English,
				SystemLanguage.Russian, Russian,
				SystemLanguage.Polish, Polish,
				
				SystemLanguage.Afrikaans, "AF",
				SystemLanguage.Arabic, "AM",
				SystemLanguage.Basque, "EU",
				SystemLanguage.Belarusian, Belarusian,
				SystemLanguage.Bulgarian, "BG",
				SystemLanguage.Catalan, "CA",

				SystemLanguage.Chinese, "ZH",
				SystemLanguage.ChineseSimplified, "zh-CHS",
				SystemLanguage.ChineseTraditional, "zh-CHT",

				SystemLanguage.Czech, "CS",
				SystemLanguage.Danish, "DA",
				SystemLanguage.Dutch, "NL",
				SystemLanguage.Estonian, "ET",
				SystemLanguage.Faroese, "FO",
				SystemLanguage.Finnish, "FI",
				SystemLanguage.French, "FR",
				SystemLanguage.German, German,
				SystemLanguage.Greek, "EL",
				SystemLanguage.Hebrew, "HE",
				SystemLanguage.Hungarian, "HU",
				SystemLanguage.Icelandic, "IS",
				SystemLanguage.Indonesian, "ID",
				SystemLanguage.Italian, "IT",
				SystemLanguage.Japanese, "JA",
				SystemLanguage.Korean, "KO",
				SystemLanguage.Latvian, "LV",
				SystemLanguage.Lithuanian, "LT",
				SystemLanguage.Norwegian, "NO",
				SystemLanguage.Portuguese, "PT",
				SystemLanguage.Romanian, "RO",
				SystemLanguage.SerboCroatian, "SH",
				SystemLanguage.Slovak, "SK",
				SystemLanguage.Slovenian, "SL",
				SystemLanguage.Spanish, "ES",
				SystemLanguage.Swedish, "SV",
				SystemLanguage.Thai, "TH",
				SystemLanguage.Turkish, "TR",
				SystemLanguage.Ukrainian, "UK",
				SystemLanguage.Vietnamese, "VI",

                SystemLanguage.Unknown, "UN",
			};

			for (var i = 0; i < codes.Length / 2; i++) 
			{
                var languageCode = (string) codes[i * 2 + 1];
                var systemLanguage = (SystemLanguage) codes[i * 2];
                LanguageToLanguageCode.Add(systemLanguage, languageCode);
                LanguageCodeToLanguage.Add(languageCode, systemLanguage);
			}
			
		}
		
        // ----------------------------------------------------------------
		
        public static string GetLanguageCode(SystemLanguage language, string defaultValue = null) 
	        => LanguageToLanguageCode.ContainsKey(language) ? LanguageToLanguageCode[language] : defaultValue;
        
        public static SystemLanguage GetLanguage(string languageCode, SystemLanguage defaultValue = DefaultLanguage) 
	        => LanguageCodeToLanguage.ContainsKey(languageCode) ? LanguageCodeToLanguage[languageCode] : defaultValue;
        
		
        // ----------------------------------------------------------------
		
	}
}
