using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSettings : CustomUIElement
{
    private const float MAXBACKGROUNDMUSIC = 0.15f;
    private const float MAXSFX = 0.25f;
    private const float DEFAULT_VALUE = 0.5f;
    private const string AUDIO_SAVEFILE_NAME = "audioSettings";

    [SerializeField, ShowInInspector, Required]
    private AudioMixer audioMixer;

    [SerializeField, ShowInInspector, Required]
    private CustomUISlider masterVolumeSlider;

    [SerializeField, ShowInInspector, Required]
    private CustomUISlider musicVolumeSlider;

    [SerializeField, ShowInInspector, Required]
    private CustomUISlider sfxVolumeSlider;

    public void SetUp()
    {
        SetupVolumeSliders();
        SetDefaultVolume();
    }

    private void SetupVolumeSliders()
    {
        masterVolumeSlider.ValueChanged += (OnMasterSliderChanged);

        musicVolumeSlider.ValueChanged += (OnMusicSliderChanged);

        sfxVolumeSlider.ValueChanged += (OnSFXSliderChanged);
    }

    private void SetDefaultVolume()
    {
        AudioSettingsDTO audioDTO = new();

        if (SaveFileUtils.TryLoadClassFromJsonFile<AudioSettingsDTO>(ref audioDTO, AUDIO_SAVEFILE_NAME))
        {
            SetMasterVolume(audioDTO.masterVolume, true);
            SetMusicVolume(audioDTO.musicVolume, true);
            SetSFXVolume(audioDTO.sfxVolume, true);
            return;
        }

        SetMasterVolume(DEFAULT_VALUE, true);
        SetMusicVolume(DEFAULT_VALUE, true);
        SetSFXVolume(DEFAULT_VALUE, true);
    }

    private void OnMasterSliderChanged(float value) => SetMasterVolume(value);
    private void OnMusicSliderChanged(float value) => SetMusicVolume(value);
    private void OnSFXSliderChanged(float value) => SetSFXVolume(value);

    private void SetMasterVolume(float value, bool includeSlider = false)
    {
        if(includeSlider) masterVolumeSlider.value = value;

        value *= 3;
        value = CalculateDecibel(value);
        audioMixer.SetFloat("MasterVol", value);

        SaveAudioSettings();
    }

    private void SetMusicVolume(float value, bool includeSlider = false)
    {
        if (includeSlider) musicVolumeSlider.value = value;
        
        value *= MAXBACKGROUNDMUSIC;
        value = CalculateDecibel(value);
        audioMixer.SetFloat("MusicVol", value);

        SaveAudioSettings();
    }

    private void SetSFXVolume(float value, bool includeSlider = false)
    {
        if(includeSlider) sfxVolumeSlider.value = value;

        value *= MAXSFX;
        value = CalculateDecibel(value);
        audioMixer.SetFloat("SFXVol", value);

        SaveAudioSettings();
    }

    private void SaveAudioSettings()
    {
        AudioSettingsDTO audioDTO = new AudioSettingsDTO()
        {
            masterVolume = masterVolumeSlider.value,
            musicVolume = musicVolumeSlider.value,
            sfxVolume = sfxVolumeSlider.value
        };

        SaveFileUtils.SaveClassToJson<AudioSettingsDTO>(audioDTO, AUDIO_SAVEFILE_NAME);
    }

    private float CalculateDecibel(float value)
    {
        return Mathf.Log10(value) * 20.0f;
    }
}
