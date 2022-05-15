// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using UnityEngine;

#endregion

namespace Amax.Audio
{

    public class MusicPlayer: MonoBehaviour
    {
        
        public static IMusicPlayer Instance { get; set; }

        private void Awake()
        {
            Instance ??= GetComponent<IMusicPlayer>();
        }
        
        public static event Action<EMusicPlayerState> OnStateChanged
        {
            add => Instance.OnStateChanged += value;
            remove => Instance.OnStateChanged -= value;
        }
        
        public static event Action<IMusicPlayerPlaylist> OnPlaylistChanged
        {
            add => Instance.OnPlaylistChanged += value;
            remove => Instance.OnPlaylistChanged -= value;
        }
        
        public static event Action<int, IMusicPlayerSong> OnSongChanged
        {
            add => Instance.OnSongChanged += value;
            remove => Instance.OnSongChanged -= value;
        }

        public static EMusicPlayerPlayMode PlayMode
        {
            get => Instance.PlayMode;
            set => Instance.PlayMode = value;
        }

        public static Func<IMusicPlayer, int> CustomShuffler
        {
            get => Instance.CustomShuffler;
            set => Instance.CustomShuffler = value;
        }

        public static EMusicPlayerState State => Instance.State;
        public static bool IsPlaying => Instance.IsPlaying;
        public static IMusicPlayerPlaylist Playlist => Instance.Playlist;
        public static IMusicPlayerSong Song => Instance.Song;
        public static int SongIndex => Instance.SongIndex;

        public static void Play(int songIndex)
            => Instance.Play(songIndex);
        
        public static void Play(IMusicPlayerPlaylist playlist, int songIndex = -1)
            => Instance.Play(playlist, songIndex);

        public static void Play()
            => Instance.Play();

        public static void Stop()
            => Instance.Stop();

        public static void Pause()
            => Instance.Pause();

        public static void Resume()
            => Instance.Resume();

        public static void PlayNext()
            => Instance.PlayNext();

        public static void PlayPrevious()
            => Instance.PlayPrevious();

        public static void PlayAgain()
            => Instance.PlayAgain();

        public static void SetPlaylist(IMusicPlayerPlaylist playlist)
            => Instance.SetPlaylist(playlist);

    }
}