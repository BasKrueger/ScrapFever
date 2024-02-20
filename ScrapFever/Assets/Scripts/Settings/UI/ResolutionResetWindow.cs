using Sirenix.OdinInspector;
using System.Collections;
using TMPro;
using UnityEngine;

public class ResolutionResetWindow : CustomUIElement
{
    public delegate void del();

    [SerializeField]
    [FilePath(ParentFolder = "Assets/Resources", Extensions = ".csv", IncludeFileExtension = false, RequireExistingPath = true)]
    private string filePath;

    [SerializeField]
    private TextMeshProUGUI text;

    private Resolution previousRes;
    private del SaveSettings;
    private del DisplayCurrentSettings;

    public void Show(Resolution toDisplay, del SaveSettings, del DisplayCurrentSettings)
    {
        previousRes = new Resolution();
        previousRes.width = Screen.width;
        previousRes.height = Screen.height;

        Screen.SetResolution(toDisplay.width, toDisplay.height, Screen.fullScreenMode);

        gameObject.SetActive(true);
        this.SaveSettings = SaveSettings;
        this.DisplayCurrentSettings = DisplayCurrentSettings;

        var glueText = CSVLanguageFileParser.GetLangDictionary(filePath, SelectedLanguage.value)["GlueResolutionConfirmation"];
        text.text = glueText.Replace("<m1>", "30");

        StartCoroutine(AutoResetTimer());

        base.Show();
    }

    protected override void HideStarted()
    {
        Screen.SetResolution(previousRes.width, previousRes.height, Screen.fullScreenMode);
        DisplayCurrentSettings();
        base.HideStarted();
    }

    public void Confirm()
    {
        StopAllCoroutines();
        SaveSettings();

        previousRes = new Resolution();
        previousRes.width = Screen.width;
        previousRes.height = Screen.height;

        Hide();
    }

    public void Cancel()
    {
        Hide();
    }

    private IEnumerator AutoResetTimer()
    {
        var timer = 30;
        var glueText = CSVLanguageFileParser.GetLangDictionary(filePath, SelectedLanguage.value)["GlueResolutionConfirmation"];
        text.text = glueText.Replace("<m1>", timer.ToString());

        while (timer > 0)
        {
            text.text = glueText.Replace("<m1>", timer.ToString());
            timer--;

            yield return new WaitForSecondsRealtime(1);
        }

        Cancel();
    }
}
