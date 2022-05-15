// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using UnityEngine;

#endregion

namespace Amax.Audio {
    
    [DefaultExecutionOrder(-100)]
    public class SoundEffectPlayer: MonoBehaviour, IEventBusListener<PlaySoundEffectRequest>
    {
        
        public static ISoundEffectPlayer Instance { get; set; }

        public static void Play(AudioClip clip)
        {
            Instance.Play(clip);
        }

        public static void Play(AudioClip clip, Vector3 position, float volume = 1f)
        {
            Instance.Play(clip, position, volume);
        }

        public static void PlayAtCameraPosition(AudioClip clip, float volume = 1f)
        {
            Instance.PlayAtCameraPosition(clip, volume);
        }
        
        private void Awake() 
        {
            Instance ??= GetComponent<ISoundEffectPlayer>();
            EventBus.AddListener(this);
        }

        private void OnDestroy() {
            EventBus.RemoveListener(this);
            Instance = null;
        }
        
        public void OnEvent(PlaySoundEffectRequest data) 
        {
            Play(data.AudioClip, data.Position, data.Volume);
        }
        
    }

}
