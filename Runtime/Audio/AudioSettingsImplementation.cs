// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using UnityEngine;

#endregion

namespace Amax.Audio {

    public class AudioSettingsImplementation: MonoBehaviour, IAudioSettings 
    {

	    [field: SerializeField] private AudioConfiguration Configuration { get; set; }
	    
        // ----------------------------------------------------------
        
        private const float MinVolumeValue = 0f;
        private const float MaxVolumeValue = 1f;
        private const float UninitializedVolumeValue = -1f;

        // ----------------------------------------------------------

        private float _masterVolume = UninitializedVolumeValue;
		public float MasterVolume 
		{
			get 
			{ 
				if (_masterVolume < 0)
				{
					_masterVolume = GetSettingFromPlayerPrefs(ESettingType.SoundEffects, Configuration.SettingsInitialValues.MasterVolume);
				}
				return _masterVolume;
			}
			set 
			{ 
				_masterVolume = Mathf.Clamp(value, MinVolumeValue, MaxVolumeValue);
				SetSetting(ESettingType.Master, _masterVolume);
				SendUpdateEvent();
			}
		}

        // ----------------------------------------------------------

        private float _soundEffectsVolume = UninitializedVolumeValue;
        public float SoundEffectsVolume 
        {
	        get 
	        { 
		        if (_soundEffectsVolume < 0)
		        {
			        _soundEffectsVolume = GetSettingFromPlayerPrefs(ESettingType.SoundEffects, Configuration.SettingsInitialValues.SoundEffectsVolume);
		        }
		        return _soundEffectsVolume;
	        }
	        set 
	        { 
		        _soundEffectsVolume = Mathf.Clamp(value, MinVolumeValue, MaxVolumeValue);
		        SetSetting(ESettingType.SoundEffects, _soundEffectsVolume);
		        SendUpdateEvent();
	        }
		}

        // ----------------------------------------------------------

        private float _musicVolume = UninitializedVolumeValue;
		public float MusicVolume 
		{
			get 
			{ 
				if (_musicVolume < 0)
				{
					_musicVolume = GetSettingFromPlayerPrefs(ESettingType.Music, Configuration.SettingsInitialValues.MusicVolume);
				}
				return _musicVolume;
			}
			set 
			{ 
				_musicVolume = Mathf.Clamp(value, MinVolumeValue, MaxVolumeValue);
				SetSetting(ESettingType.Music, _musicVolume);
				SendUpdateEvent();
			}
		}
		
		// ----------------------------------------------------------

		private float _dialogsVolume = UninitializedVolumeValue;
		
		public float DialogsVolume 
		{
			get 
			{ 
				if (_dialogsVolume < 0)
				{
					_dialogsVolume = GetSettingFromPlayerPrefs(ESettingType.Dialogs, Configuration.SettingsInitialValues.DialogsVolume);
				}
				return _dialogsVolume;
			}
			set 
			{ 
				_dialogsVolume = Mathf.Clamp(value, MinVolumeValue, MaxVolumeValue);
				SetSetting(ESettingType.Dialogs, _dialogsVolume);
				SendUpdateEvent();
			}
		}

        // ----------------------------------------------------------

        private static void SendUpdateEvent() 
        {
            EventBus.RaiseEvent(new OnAudioSettingsChanged());
        }

        public float CombinedMusicVolume => MasterVolume * MusicVolume;
        public float CombinedSoundEffectsVolume => MasterVolume * SoundEffectsVolume;
        public float CombinedDialogsVolume => MasterVolume * DialogsVolume;
        
        private enum ESettingType
        {
	        Master,
	        SoundEffects,
	        Music,
	        Dialogs
        }

        private string GetSettingPlayerPrefsKey(ESettingType type)
	        => $"Amax.Audio.Settings.{type}";

        private float GetSettingFromPlayerPrefs(ESettingType type, float defaultValue)
	        => PlayerPrefs.GetFloat(GetSettingPlayerPrefsKey(type), defaultValue);

        private void SetSetting(ESettingType type, float value)
        {
	        PlayerPrefs.SetFloat(GetSettingPlayerPrefsKey(type), value);
	        PlayerPrefs.Save();
        }

    }

}
