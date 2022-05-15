// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using UnityEngine;

#endregion

namespace Amax.Audio
{
    public class SoundEffectPlayerImplementation: MonoBehaviour, ISoundEffectPlayer
    {
        
        private const string LogTag = "SoundEffectsPlayerImplementation:";
        
        public void Play(AudioClip clip, Vector3 position, float volume) 
        {
            if (clip == null) 
            {
                Debug.LogWarning($"{LogTag} Play() clip == null");
                return;
            }
            AudioSource.PlayClipAtPoint(clip, position, volume * AudioSettings.CombinedSoundEffectsVolume);
        }
        
    }
}