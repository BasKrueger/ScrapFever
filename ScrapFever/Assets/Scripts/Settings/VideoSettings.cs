using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoSettings : CustomUIElement
{
    private const string VIDEO_SAVEFILE_NAME = "videoSettings";

    [SerializeField, ShowInInspector, Required]
    private CustomUISlider brightness;

    [SerializeField, ShowInInspector, Required]
    private LeftRightSelection displayMode;

    [SerializeField, ShowInInspector, Required]
    private LeftRightSelection quality;

    [SerializeField, ShowInInspector, Required]
    private Dropdown availableResolutions;

    [SerializeField, ShowInInspector, Required]
    private LeftRightSelection vSync;

    [SerializeField]
    private ResolutionResetWindow resolutionConfirmationWindow;

    private void Awake()
    {
        brightness.ValueChanged += OnBrightnessChanged;
        displayMode.ValueChanged += OnDisplayModeChanged;
        quality.ValueChanged += OnQualityChanged;
        availableResolutions.onValueChanged.AddListener(OnResolutionChanged);
        vSync.ValueChanged += OnVSyncChanged;
    }

    public void SetUp()
    {
        List<string> resolutionsAsStrings = new();

        foreach (var res in Screen.resolutions)
        {
            var str = Mathf.RoundToInt(res.width) + "X" + Mathf.RoundToInt(res.height);
            if (!resolutionsAsStrings.Contains(str))
            {
                resolutionsAsStrings.Add(str);
            }
        }

        availableResolutions.AddOptions(resolutionsAsStrings);

        LoadSettings();
    }

    #region OnValueChanged
    private void OnBrightnessChanged(float value)
    {
        Brightness.SetBrightness(value);
        SaveSettings();
    }

    private void OnDisplayModeChanged(float value)
    {
        var currentRes = Screen.currentResolution;
        FullScreenMode mode = FullScreenMode.FullScreenWindow;

        switch (value)
        {
            case 0:
                mode = FullScreenMode.Windowed;
                break;
            case 1:
                mode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 2:
                mode = FullScreenMode.FullScreenWindow;
                break;
        }

        Screen.SetResolution(currentRes.width, currentRes.height, mode);
        SaveSettings();
    }

    private void OnQualityChanged(float value)
    {
        QualitySettings.SetQualityLevel(Mathf.RoundToInt(value));
        SaveSettings();
    }

    private void OnResolutionChanged(int value)
    {
        var newRes = availableResolutions.options[availableResolutions.value].text.TryParseToResolution();

        if(newRes.width == Screen.width && newRes.height == Screen.height)
        {
            return;
        }

        DisplayCurrentSettings();
        resolutionConfirmationWindow.Show(newRes, SaveSettings, DisplayCurrentSettings);
    }

    private void OnVSyncChanged(float value)
    {
        QualitySettings.vSyncCount = Mathf.RoundToInt(value);
        SaveSettings();
    }
    #endregion

    private void DisplayCurrentSettings()
    {
        brightness.value = Brightness.CurrentBrightness;

        quality.currentIndex = QualitySettings.GetQualityLevel();
        quality.UpdateText();

        vSync.currentIndex = QualitySettings.vSyncCount == 0 ? 0 : 1;
        vSync.UpdateText();

        #region Display resolution
        for (int i = 0; i < availableResolutions.options.Count; i++)
        {
            var res = availableResolutions.options[i].text.TryParseToResolution();
            if (res.width == Screen.width && res.height == Screen.height)
            {
                availableResolutions.value = i;
                availableResolutions.RefreshShownValue();
                break;
            }
        }

        #endregion

        #region Display DisplayMode
        switch (Screen.fullScreenMode)
        {
            case FullScreenMode.ExclusiveFullScreen: // FullScreen
                displayMode.currentIndex = 2;
                break;
            case FullScreenMode.FullScreenWindow: //Borderless
                displayMode.currentIndex = 1;
                break;
            case FullScreenMode.Windowed: //Windowed
                displayMode.currentIndex = 0;
                break;
        }
        displayMode.UpdateText();
        #endregion
    }

    private void LoadSettings()
    {
        if (Application.isMobilePlatform) return;

        VideoSettingsDTO dto = new();
        if (SaveFileUtils.TryLoadClassFromJsonFile(ref dto, VIDEO_SAVEFILE_NAME))
        {
            Brightness.SetBrightness(dto.brightness);
            QualitySettings.SetQualityLevel(dto.quality);
            QualitySettings.vSyncCount = dto.vSync ? 0 : 1;

            Resolution res = dto.resolution.TryParseToResolution();
            Screen.SetResolution(res.width, res.height, dto.fullScreenMode, res.refreshRateRatio);
        }
        else
        {
            Brightness.SetBrightness(1);
            QualitySettings.SetQualityLevel(1);
            QualitySettings.vSyncCount = 0;
        }

        DisplayCurrentSettings();
    }

    private void SaveSettings()
    {
        VideoSettingsDTO dto = new()
        {
            fullScreenMode = Screen.fullScreenMode,
            resolution = $"{Screen.width}X{Screen.height}",
            quality = quality.currentIndex,
            vSync = vSync.currentIndex == 0,
            brightness = brightness.value
        };

        SaveFileUtils.SaveClassToJson(dto, VIDEO_SAVEFILE_NAME);
    }
}
