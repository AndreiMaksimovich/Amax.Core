// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using Amax.Localization;
using UnityEngine;

#endregion

namespace Amax.Audio
{
    
    [CreateAssetMenu(menuName = "Amax/Audio/MusicPlayer - Playlist", fileName = "Playlist.asset")]
    public class MusicPlayerPlaylist: ScriptableObject, IMusicPlayerPlaylist
    {
        [SerializeField] private MusicPlayerSong[] songs;
        [field: SerializeField] public string Title { get; set; }
        [field: SerializeField] public LocalizedStringDictionary LocalizedTitle { get; set; }
        
        public IMusicPlayerSong[] Songs => songs;

        public void SetSongs(MusicPlayerSong[] songs)
        {
            this.songs = songs;
        }
        
        public string GetTitle(SystemLanguage language = SystemLanguage.Unknown)
        {
            if (language == SystemLanguage.Unknown) language = TextLocalization.Language;
            var title = LocalizedTitle.Get(language, TextLocalization.DefaultLanguage, null);
            return string.IsNullOrEmpty(title) ? Title : title;
        }
    }
}