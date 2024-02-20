using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSettings : CustomUIElement
{
    [SerializeField, ShowInInspector, Required]
    private LeftRightSelection languageOption;

    [SerializeField, ShowInInspector, Required]
    private ConfirmationScreens deleteConfirmation;

    private const string GAMESETTINGS_SAVEFILE_NAME = "gameSettings";

    private void Awake()
    {
        languageOption.ValueChanged += ApplyLanguage;
        deleteConfirmation.confirmed += OnDeleteConfirmed;
    }

    public void SetUp()
    {
        LoadDefaultLanguage();
    }

    private void LoadDefaultLanguage()
    {
        GameSettingsDTO dto = new();
        SaveFileUtils.TryLoadClassFromJsonFile<GameSettingsDTO>(ref dto, GAMESETTINGS_SAVEFILE_NAME);
        SelectedLanguage.value = dto != null ? dto.language : LanguageType.EN;

        foreach (var text in FindObjectsOfType<TextByLanguage>(true))
        {
            text.SetTextsInUI();
        }

        switch (SelectedLanguage.value)
        {
            case LanguageType.None:
                break;
            case LanguageType.DE:
                languageOption.currentIndex = 1;
                break;
            case LanguageType.EN:
                languageOption.currentIndex = 0;
                break;
            case LanguageType.FR:
                break;
        }

        languageOption.UpdateText();

        foreach (var text in FindObjectsOfType<TextByLanguage>(true))
        {
            text.SetTextsInUI();
        }
    }

    public void ApplyLanguage(float value)
    {
        GameSettingsDTO dto = new();

        //Apply language Settings
        switch (value)
        {
            case 0:
                SelectedLanguage.value = LanguageType.EN;
                break;
            case 1:
                SelectedLanguage.value = LanguageType.DE;
                break;
        }

        dto.language = SelectedLanguage.value;

        SaveFileUtils.SaveClassToJson<GameSettingsDTO>(dto, GAMESETTINGS_SAVEFILE_NAME);

        foreach (var text in FindObjectsOfType<TextByLanguage>(true))
        {
            text.SetTextsInUI();
        }

        foreach(var leftRightButton in FindObjectsOfType<LeftRightSelection>(true))
        {
            leftRightButton.UpdateText();
        }
    }

    public void OnDeleteSaveClicked()
    {
        deleteConfirmation.Show();
    }

    private void OnDeleteConfirmed()
    {
        string[] filenames = new string[]
        {
            "Settings",
            "videoSettings",
            "audioSettings",
            "gameSettings"
        };

        foreach (string filename in filenames)
        {
            SaveFileUtils.DeleteSaveFileBinary(filename);
            SaveFileUtils.DeleteSaveFileJSON(filename);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //deleteOkay.Setup("Settings", "videoSettings", "audioSettings", "gameSettings");
    }
}
