// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

#endregion

namespace Amax.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicPlayerImplementation : MonoBehaviour, IMusicPlayer, IEventBusListener<OnAudioSettingsChanged>
    {
        public event Action<EMusicPlayerState> OnStateChanged;
        public event Action<IMusicPlayerPlaylist> OnPlaylistChanged;
        public event Action<int, IMusicPlayerSong> OnSongChanged;
        
        public EMusicPlayerPlayMode PlayMode { get; set; } = EMusicPlayerPlayMode.LoopPlaylist;
        public Func<IMusicPlayer, int> CustomShuffler { get; set; } = null;
        
        
        private EMusicPlayerState _state = EMusicPlayerState.NotInitialized;
        public EMusicPlayerState State
        {
            get => _state;
            private set
            {
                if (_state == value) return;
                _state = value;
                OnStateChanged?.Invoke(_state);
            }
        } 
        
        public bool IsPlaying => State == EMusicPlayerState.IsPlaying;

        private IMusicPlayerPlaylist _playlist;

        public IMusicPlayerPlaylist Playlist
        {
            get => _playlist;
            private set
            {
                if (_playlist == value) return;
                _playlist = value;
                OnPlaylistChanged?.Invoke(_playlist);
            }
        }
        
        public IMusicPlayerSong Song 
            => IsSongIndexValid(SongIndex) ? Playlist.Songs[SongIndex] : null;

        private int _songIndex = -1;

        public int SongIndex
        {
            get => _songIndex;
            set
            {
                if (_songIndex == value) return;
                _songIndex = value;
                if (IsSongIndexValid(_songIndex))
                {
                    OnSongChanged?.Invoke(_songIndex, Song);
                }
            }
        }
        private Command _command = null;
        private AudioSource AudioSource { get; set; }

        private void Awake()
        {
            EventBus.AddListener(this as IEventBusListener<OnAudioSettingsChanged>);
            AudioSource = GetComponent<AudioSource>();
            UpdateAudioSourceParameters();
            StartCoroutine(LifeCycleCoroutine());
        }

        private void OnDestroy()
        {
            EventBus.RemoveListener(this as IEventBusListener<OnAudioSettingsChanged>);
            if (State != EMusicPlayerState.NotInitialized)
            {
                Song?.AssetReference.ReleaseAsset();
            }
        }

        public void Play(int songIndex)
        {
            if (State == EMusicPlayerState.NotInitialized) return;
            _command = new Command()
            {
                type = ECommandType.Play,
                songIndex = songIndex
            };
        }
        
        public void Play(IMusicPlayerPlaylist playlist, int songIndex = -1)
        {
            _command = new Command()
            {
                type = ECommandType.Play,
                playlist = playlist,
                songIndex = songIndex
            };
        }

        public void Play()
        {
            if (State == EMusicPlayerState.NotInitialized) return;
            _command = new Command()
            {
                type = ECommandType.Play,
                songIndex = (IsSongIndexValid(SongIndex)) ? SongIndex : -1
            };
        }

        public void Stop()
        {
            if (State == EMusicPlayerState.NotInitialized) return;
            _command = new Command()
            {
                type = ECommandType.Stop
            };
        }

        public void Pause()
        {
            if (State != EMusicPlayerState.IsPlaying) return;
            _command = new Command()
            {
                type = ECommandType.Pause
            };
        }

        private bool IsSongIndexValid(int songIndex)
            => songIndex >= 0 && songIndex < Playlist.Songs.Length;

        private bool IsPlaylistValid(IMusicPlayerPlaylist playlist)
            => playlist != null && playlist.Songs != null && playlist.Songs.Length > 0 && playlist.Songs[0].AssetReference.RuntimeKeyIsValid();

        private int GetFirstSongIndex(int songIndex)
        {
            if (IsSongIndexValid(songIndex)) return songIndex;
            switch (PlayMode)
            {
                case EMusicPlayerPlayMode.Shuffle:
                    return Random.Range(0, Playlist.Songs.Length);
                case EMusicPlayerPlayMode.CustomShuffle:
                    return CustomShuffler.Invoke(this);
                case EMusicPlayerPlayMode.LoopSong:
                    return IsSongIndexValid(songIndex) ? SongIndex : 0;
                case EMusicPlayerPlayMode.LoopPlaylist: 
                default:
                    return 0;
            }
        }

        private int GetNextSongIndex()
        {
            switch (PlayMode)
            {
                case EMusicPlayerPlayMode.Shuffle:
                    return Random.Range(0, Playlist.Songs.Length);
                case EMusicPlayerPlayMode.CustomShuffle:
                    return CustomShuffler.Invoke(this);
                case EMusicPlayerPlayMode.LoopSong:
                    return SongIndex>=0 ? SongIndex : 0;
                case EMusicPlayerPlayMode.LoopPlaylist: 
                default:
                    return SongIndex + 1 >= Playlist.Songs.Length ? 0 : SongIndex + 1;
            }
        }

        private int GetPreviousSongIndex()
        {
            switch (PlayMode)
            {
                case EMusicPlayerPlayMode.Shuffle:
                    return Random.Range(0, Playlist.Songs.Length);
                case EMusicPlayerPlayMode.CustomShuffle:
                    return CustomShuffler.Invoke(this);
                case EMusicPlayerPlayMode.LoopSong:
                    return SongIndex>=0 ? SongIndex : 0;
                case EMusicPlayerPlayMode.LoopPlaylist: 
                default:
                    return SongIndex - 1 < 0 ? Playlist.Songs.Length - 1 : SongIndex - 1;
            }
        }

        public void PlayNext()
        {
            if (State == EMusicPlayerState.NotInitialized) return;
            _command = new Command()
            {
                type = ECommandType.Play,
                songIndex = GetNextSongIndex()
            };
        }

        public void PlayPrevious()
        {
            if (State == EMusicPlayerState.NotInitialized) return;
            _command = new Command()
            {
                type = ECommandType.Play,
                songIndex = GetPreviousSongIndex()
            };
        }

        public void Resume()
        {
            if (State != EMusicPlayerState.Paused) return;
            _command = new Command()
            {
                type = ECommandType.Resume
            };
        }

        public void PlayAgain()
        {
            if (State != EMusicPlayerState.IsPlaying) return;
            _command = new Command()
            {
                type = ECommandType.PlayAgain
            };
        }

        public void SetPlaylist(IMusicPlayerPlaylist playlist)
        {
            _command = new Command()
            {
                type = IsPlaying && playlist != null ? ECommandType.Play : ECommandType.SetPlaylist,
                playlist = playlist
            };
        }

        private IEnumerator LifeCycleCoroutine()
        {

            while (true)
            {
                
                if (_command == null)
                {
                    // Play next song
                    if (State == EMusicPlayerState.IsPlaying && !AudioSource.isPlaying)
                    {
                        PlayNext();
                    }
                    yield return null;
                    continue;
                }
          

                var command = _command;
                _command = null;
                
                switch (command.type)
                {
                    case ECommandType.SetPlaylist:
                        if (Playlist != null && State!=EMusicPlayerState.NotInitialized && IsSongIndexValid(SongIndex))
                        {
                            yield return UnloadSongCoroutine(Song);
                        }
                        AudioSource.clip = null;
                        Playlist = command.playlist;
                        SongIndex = -1;
                        State = EMusicPlayerState.NotInitialized;
                        break;
                    
                    case ECommandType.Play:
                        yield return PlayCommandCoroutine(command);
                        break;
                    
                    case ECommandType.PlayAgain:
                        if (State == EMusicPlayerState.IsPlaying)
                        {
                            AudioSource.time = 0;
                        }
                        break;
                    
                    case ECommandType.Stop:
                        if (State != EMusicPlayerState.NotInitialized)
                        {
                            AudioSource.Stop();
                            State = EMusicPlayerState.Stopped;
                        }

                        break;
                    
                    case ECommandType.Pause:
                        if (State == EMusicPlayerState.IsPlaying)
                        {
                            AudioSource.Pause();
                            State = EMusicPlayerState.Paused;
                        }
                        break;
                    
                    case ECommandType.Resume:
                        if (State == EMusicPlayerState.Paused)
                        {
                            AudioSource.UnPause();
                            State = EMusicPlayerState.IsPlaying;
                        }
                        break;
                }

            }
            
        }

        private IEnumerator BeforeSongUnloadCoroutine()
        {
            yield return null;
        }
        
        private IEnumerator PlayCommandCoroutine(Command command)
        {
            var playlistChanged = false;
            
            // Change playlist
            if (command.playlist != null && Playlist != command.playlist)
            {
                // Stop AS before unloading song
                if (State == EMusicPlayerState.IsPlaying)
                {
                    AudioSource.Stop();
                    yield return BeforeSongUnloadCoroutine();
                }

                if (State != EMusicPlayerState.NotInitialized)
                {
                    yield return UnloadSongCoroutine(Song);
                }
                
                Playlist = command.playlist;
                command.songIndex = GetFirstSongIndex(command.songIndex);
                playlistChanged = true;
            }

            // Is playlist valid
            if (!IsPlaylistValid(Playlist))
            {
                State = EMusicPlayerState.NotInitialized;
                SongIndex = -1;
                yield break;
            }
            
            var targetSongIndex = IsSongIndexValid(command.songIndex) ? command.songIndex : GetNextSongIndex();

            if (playlistChanged || targetSongIndex != SongIndex)
            {
                // Stop AS before unloading song
                if (!playlistChanged && State == EMusicPlayerState.IsPlaying)
                {
                    AudioSource.Stop();
                    yield return BeforeSongUnloadCoroutine();
                }
            
                // Unload
                if (!playlistChanged && State != EMusicPlayerState.NotInitialized && IsSongIndexValid(SongIndex))
                {
                    yield return UnloadSongCoroutine(Song);
                }
            
                // Load
                yield return LoadSongCoroutine(Playlist.Songs[targetSongIndex]);
            }
            
            SongIndex = targetSongIndex;
            AudioSource.clip = Song.AssetReference.Asset as AudioClip;
            AudioSource.Play();
                
            State = EMusicPlayerState.IsPlaying;
        }
        
        private IEnumerator UnloadSongCoroutine(IMusicPlayerSong song)
        {
            song?.AssetReference?.ReleaseAsset();
            yield break;
        }

        private IEnumerator LoadSongCoroutine(IMusicPlayerSong song, Action<AudioClip, Exception> onComplete = null)
        {
            var taskHandler = song.AssetReference.LoadAssetAsync();
            yield return taskHandler;
            onComplete?.Invoke(taskHandler.Result, taskHandler.OperationException);
        }
        
        public void OnEvent(OnAudioSettingsChanged data)
        {
            UpdateAudioSourceParameters();
        }

        private void UpdateAudioSourceParameters()
        {
            AudioSource.volume = AudioSettings.MusicVolume;
        }
        
        // /\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\
        
        private class Command
        {
            public ECommandType type;
            public int songIndex;
            public IMusicPlayerPlaylist playlist;
        }
        
        private enum ECommandType
        {
            Play,
            PlayAgain,
            Resume,
            Stop,
            Pause,
            SetPlaylist
        }

        
        
    }
}