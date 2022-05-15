// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using Amax.Audio;
using Amax.Navigation;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;

#endregion

namespace Amax.Core.Examples
{

    public class SceneControllerExampleAudio : ASceneController, IEventBusListener<OnAudioSettingsChanged>
    {

        public Button
            buttonPlay,
            buttonStop,
            buttonPause,
            buttonPlayNext,
            buttonPlayPrevious,
            buttonPlayAgain,
            buttonResume;

        public Button
            buttonChangePlaylist,
            buttonChangePlaylistToNull;

        public TMP_Dropdown
            dropdownPlaymode;

        private IMusicPlayer _musicPlayer;
        public MusicPlayerPlaylist playlist_01, playlist_02;

        public Slider volumeSlider;

        protected override void Awake()
        {
            base.Awake();
            
            EventBus.AddListener(this as IEventBusListener<OnAudioSettingsChanged>);

            _musicPlayer ??= MusicPlayer.Instance;

            buttonPlay.onClick.AddListener(OnButtonClick_Play);
            buttonStop.onClick.AddListener(OnButtonClick_Stop);
            buttonPause.onClick.AddListener(OnButtonClick_Pause);
            buttonPlayNext.onClick.AddListener(OnButtonClick_PlayNext);
            buttonPlayPrevious.onClick.AddListener(OnButtonClick_PlayPrevious);
            buttonPlayAgain.onClick.AddListener(OnButtonClick_PlayAgain);
            buttonResume.onClick.AddListener(OnButtonClick_Resume);

            buttonChangePlaylist.onClick.AddListener(OnButtonCLick_ChangePlaylist);
            buttonChangePlaylistToNull.onClick.AddListener(() => _musicPlayer.SetPlaylist(null));

            foreach (EMusicPlayerPlayMode mode in Enum.GetValues(typeof(EMusicPlayerPlayMode)))
            {
                dropdownPlaymode.options.Add(new TMP_Dropdown.OptionData(mode.ToString()));
            }

            dropdownPlaymode.value = (int) _musicPlayer.PlayMode;
            dropdownPlaymode.onValueChanged.AddListener(OnDropdownPlaymodeValueChanged);

            volumeSlider.value = AudioSettings.MusicVolume;
            volumeSlider.onValueChanged.AddListener(OnVolumeSliderValueChanged);
        }

        private void OnDestroy()
        {
            EventBus.RemoveListener(this as IEventBusListener<OnAudioSettingsChanged>);
        }

        private void OnVolumeSliderValueChanged(float value)
        {
            AudioSettings.MusicVolume = value;
        }

        private int CustomShuffler(IMusicPlayer mp)
            => Random.Range(0, mp.Playlist.Songs.Length);

        private void Start()
        {
            _musicPlayer.CustomShuffler = CustomShuffler;
        }

        private void OnDropdownPlaymodeValueChanged(int value)
        {
            _musicPlayer.PlayMode = (EMusicPlayerPlayMode) value;
        }

        public void OnButtonCLick_ChangePlaylist()
        {
            _musicPlayer.Play((MusicPlayerPlaylist) _musicPlayer.Playlist == playlist_01 ? playlist_02 : playlist_01);
        }

        public void OnButtonClick_Play()
        {
            _musicPlayer.Play();
        }

        public void OnButtonClick_Resume()
        {
            _musicPlayer.Resume();
        }

        public void OnButtonClick_PlayNext()
        {
            _musicPlayer.PlayNext();
        }

        public void OnButtonClick_PlayPrevious()
        {
            _musicPlayer.PlayPrevious();
        }

        public void OnButtonClick_PlayAgain()
        {
            _musicPlayer.PlayAgain();
        }

        public void OnButtonClick_Stop()
        {
            _musicPlayer.Stop();
        }

        public void OnButtonClick_Pause()
        {
            _musicPlayer.Pause();
        }
        
        public void OnEvent(OnAudioSettingsChanged data)
        {
            volumeSlider.value = AudioSettings.MusicVolume;
        }
    }

}
