// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using UnityEngine;

#endregion

namespace Amax.Audio
{
    public static class SoundEffectPlayerExtension
    {

        public static void PlayAtCameraPosition(this ISoundEffectPlayer player, AudioClip audioClip, float volume = 1f)
        {
            player.Play(audioClip, Camera.main != null ? Camera.main.transform.position : Vector3.zero, volume);
        }

        public static void Play(this ISoundEffectPlayer player, AudioClip audioClip, float volume = 1f)
        {
            player.Play(audioClip, Vector3.zero, volume);
        }

    }
}