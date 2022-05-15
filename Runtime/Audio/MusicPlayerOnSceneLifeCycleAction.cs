// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using UnityEngine;

#endregion

namespace Amax.Audio
{
    [DefaultExecutionOrder(-1)]
    public class MusicPlayerOnSceneLifeCycleAction: MonoBehaviour
    {
        
        [field: SerializeField] public ELifeCycleEventType LifeCycleEvent { get; set; }
        [field: SerializeField] public EActionType Action { get; set; }
        [field: SerializeField] public MusicPlayerPlaylist Playlist { get; set; }
        [field: SerializeField] public int SongIndex { get; set; } = -1;

        private void Start()
        {
            if (LifeCycleEvent == ELifeCycleEventType.OnSceneStart)
            {
                ProcessAction();
            }
        }

        private void OnDestroy()
        {
            if (LifeCycleEvent == ELifeCycleEventType.OnSceneDestroy)
            {
                ProcessAction();
            }
        }

        private void ProcessAction()
        {
            
            switch (Action)
            {
                case EActionType.Play:
                    MusicPlayer.Play(Playlist, SongIndex);
                    break;
                case EActionType.SetPlaylist:
                    MusicPlayer.SetPlaylist(Playlist);
                    break;
                case EActionType.StopPlaying:
                    MusicPlayer.Stop();
                    break;
            }
        }

        public enum ELifeCycleEventType
        {
            OnSceneStart,
            OnSceneDestroy
        }
        
        public enum EActionType
        {
            Play,
            SetPlaylist,
            StopPlaying
        }
        
    }
}