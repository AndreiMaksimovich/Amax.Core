// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using UnityEngine;

#endregion

namespace Amax.Audio
{
    
    public interface ISoundEffectPlayer 
    {
        void Play(AudioClip clip, Vector3 position, float volume);
    }
    
    public class PlaySoundEffectRequest : EventBusBaseEvent 
    {
        
        public Vector3 Position { get; set; }
        public float Volume { get; set; }
        public AudioClip AudioClip { get; set; }

        public PlaySoundEffectRequest(AudioClip audioClip, float volume = 1f) 
        {
            AudioClip = audioClip;
            Volume = volume;
            Position = Vector3.zero;
        }

        public static void Raise(AudioClip audioClip, float volume = 1f)
            => EventBus.RaiseEvent(new PlaySoundEffectRequest(audioClip, volume));
        
        public PlaySoundEffectRequest(AudioClip audioClip, Vector3 position, float volume = 1f) 
        {
            AudioClip = audioClip;
            Position = position;
            Volume = volume;
        }
        
        public static void Raise(AudioClip audioClip, Vector3 position, float volume = 1f)
            => EventBus.RaiseEvent(new PlaySoundEffectRequest(audioClip, position, volume));
        
    }
    
}