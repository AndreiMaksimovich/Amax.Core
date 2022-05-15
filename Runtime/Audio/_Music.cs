// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using Amax.AddressableAssets;
using UnityEngine;

#endregion

namespace Amax.Audio
{

    public interface IMusicPlayerSong
    {
        public AssetReferenceAudioClip AssetReference { get; }
        public string GetTitle(SystemLanguage language = SystemLanguage.Unknown);
    }

    public interface IMusicPlayerPlaylist
    {
        public IMusicPlayerSong[] Songs { get; }
        public string GetTitle(SystemLanguage language = SystemLanguage.Unknown);
    }

    public enum EMusicPlayerState
    {
        NotInitialized,
        Stopped,
        Paused,
        IsPlaying
    }
    
    public enum EMusicPlayerPlayMode
    {
        LoopSong,
        LoopPlaylist,
        Shuffle,
        CustomShuffle
    }

    public interface IMusicPlayer
    {

        public event Action<EMusicPlayerState> OnStateChanged;
        public event Action<IMusicPlayerPlaylist> OnPlaylistChanged;
        public event Action<int, IMusicPlayerSong> OnSongChanged; 

        public EMusicPlayerPlayMode PlayMode { get; set; }
        public Func<IMusicPlayer, int> CustomShuffler { get; set; }
        
        public EMusicPlayerState State { get; }
        public bool IsPlaying { get; }
        public IMusicPlayerPlaylist Playlist { get; }
        public IMusicPlayerSong Song { get; }
        public int SongIndex { get; }

        public void Play(int songIndex);
        public void Play(IMusicPlayerPlaylist playlist, int songIndex = -1);
        public void Play();

        public void Stop();
        public void Pause();
        public void Resume();
        
        public void PlayNext();
        public void PlayPrevious();
        public void PlayAgain();

        public void SetPlaylist(IMusicPlayerPlaylist playlist);
    }

    
    
}