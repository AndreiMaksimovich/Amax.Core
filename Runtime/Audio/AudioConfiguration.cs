// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using UnityEngine;

#endregion

namespace Amax.Audio
{
    
    [CreateAssetMenu(menuName = "Amax/Audio/Configuration", fileName = "AudioConfiguration.asset")]
    public class AudioConfiguration: ScriptableObject
    {
        
        [field: SerializeField] public AudioSettingsInitialValues SettingsInitialValues { get; set; }
        
        [Serializable]
        public class AudioSettingsInitialValues
        {
            [SerializeField] [Range(0f, 1f)] private float masterVolume = 1f;
            [SerializeField] [Range(0f, 1f)] private float soundEffectsVolume = 1f;
            [SerializeField] [Range(0f, 1f)] private float dialogsVolume = 1f;
            [SerializeField] [Range(0f, 1f)] private float musicVolume = 1f;

            public float MasterVolume
            {
                get => masterVolume;
                set => masterVolume = value;
            }
        
            public float SoundEffectsVolume
            {
                get => soundEffectsVolume;
                set => soundEffectsVolume = value;
            }
        
            public float DialogsVolume
            {
                get => dialogsVolume;
                set => dialogsVolume = value;
            }
        
            public float MusicVolume
            {
                get => musicVolume;
                set => musicVolume = value;
            }
        }
        
    }

    
    
}