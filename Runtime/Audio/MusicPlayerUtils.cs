// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System.Collections.Generic;
using Amax.AddressableAssets;
using UnityEngine;

#endregion

namespace Amax.Audio
{
    public static class MusicPlayerUtils
    {

        public static IMusicPlayerPlaylist CreatePlaylist(List<AssetReferenceAudioClip> songs, List<string> songTitles = null, string playlistTitle = null)
        {
            var mpSongs = new List<IMusicPlayerSong>();
            for (var i = 0; i < songs.Count; i++)
            {
                mpSongs.Add(
                    new Song()
                    {
                        AssetReference = songs[i],
                        Title = songTitles?[i]
                    }
                );
            }
            return new Playlist()
            {
                Songs = mpSongs.ToArray(),
                Title = playlistTitle
            };
        }
        
        private class Song : IMusicPlayerSong
        {
            public AssetReferenceAudioClip AssetReference { get; set; }
            public string Title { get; set; }
            public string GetTitle(SystemLanguage language = SystemLanguage.Unknown) => Title;
        }

        private class Playlist: IMusicPlayerPlaylist
        {
            public IMusicPlayerSong[] Songs { get; set; }
            public string Title { get; set; }
            public string GetTitle(SystemLanguage language = SystemLanguage.Unknown) => Title;
        }
        
    }
}