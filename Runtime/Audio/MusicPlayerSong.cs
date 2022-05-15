// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using Amax.AddressableAssets;
using Amax.Localization;
using UnityEngine;

#endregion

namespace Amax.Audio
{
    
    [CreateAssetMenu(menuName = "Amax/Audio/MusicPlayer - Song", fileName = "Song.asset")]
    public class MusicPlayerSong: ScriptableObject, IMusicPlayerSong
    {
        
        [field: SerializeField] public AssetReferenceAudioClip AssetReference { get; set; }
        [field: SerializeField] public string Title { get; set; }
        [field: SerializeField] public LocalizedStringDictionary LocalizedTitle { get; set; }
        
        public string GetTitle(SystemLanguage language = SystemLanguage.Unknown)
        {
            if (language == SystemLanguage.Unknown) language = TextLocalization.Language;
            var title = LocalizedTitle.Get(language, TextLocalization.DefaultLanguage, null);
            return string.IsNullOrEmpty(title) ? Title : title;
        }
    }
}