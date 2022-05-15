// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

namespace Amax.Audio {
    
    public interface IAudioSettings 
    {
        public float MasterVolume { get; set; }
        public float SoundEffectsVolume { get; set; }
        public float MusicVolume { get; set; }
        public float DialogsVolume { get; set; }
        public float CombinedMusicVolume { get; }
        public float CombinedSoundEffectsVolume { get; }
        public float CombinedDialogsVolume { get; }
    }
    
    public class OnAudioSettingsChanged : EventBusBaseEvent { }
    
}
