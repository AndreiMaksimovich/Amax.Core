// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using UnityEngine;

#endregion

namespace Amax.Audio
{
    [DefaultExecutionOrder(-100)]
    public class AudioSettings: MonoBehaviour
    {
        
        public static IAudioSettings Instance { get; set; }

        private void Awake()
        {
            Instance ??= GetComponent<IAudioSettings>();
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public static float MasterVolume { get => Instance.MasterVolume; set => Instance.MasterVolume = value; }
        public static float SoundEffectsVolume { get => Instance.SoundEffectsVolume; set => Instance.SoundEffectsVolume = value; }
        public static float MusicVolume { get => Instance.MusicVolume; set => Instance.MusicVolume = value; }
        public static float DialogsVolume { get => Instance.DialogsVolume; set => Instance.DialogsVolume = value; }
        
        public static float CombinedMusicVolume => Instance.CombinedMusicVolume;
        public static float CombinedSoundEffectsVolume => Instance.CombinedSoundEffectsVolume;
        public static float CombinedDialogsVolume => Instance.CombinedDialogsVolume;
        
    }
}