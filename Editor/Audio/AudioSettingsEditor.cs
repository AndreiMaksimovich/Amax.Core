// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using UnityEditor;
using UnityEngine;

#endregion

namespace Amax.Audio.Editor {

    [CustomEditor(typeof(AudioSettingsImplementation))]
    public class AudioSettingsEditor : UnityEditor.Editor 
    {

        public override void OnInspectorGUI() {

            base.OnInspectorGUI();
            
            if (!Application.isPlaying) 
            {
                return;
            }
            
            EditorGUI.BeginChangeCheck();

            var settings = target as AudioSettingsImplementation;

            var masterVolume = EditorGUILayout.Slider("Master Volume", settings.MasterVolume, 0f, 1f);
            var soundEffectsVolume = EditorGUILayout.Slider("Sound Effects Volume", settings.SoundEffectsVolume, 0f, 1f);
            var musicVolume = EditorGUILayout.Slider("Music Volume", settings.MusicVolume, 0f, 1f);

            if (EditorGUI.EndChangeCheck()) 
            {
                settings.MasterVolume = masterVolume;
                settings.SoundEffectsVolume = soundEffectsVolume;
                settings.MusicVolume = musicVolume;
            }
            
        }

    }

}