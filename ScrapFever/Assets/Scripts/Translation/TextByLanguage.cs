using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TextByLanguage : SerializedMonoBehaviour
{
    [Title("Texts")]
    [SerializeField, HideInPlayMode]
    private TextMeshProUGUI[] uiTextMeshes;

    [SerializeField]
    [FilePath(ParentFolder = "Assets/Resources", Extensions = ".csv", IncludeFileExtension = false, RequireExistingPath = true)]
    private string filePath;

    //[SerializeField, HideInInspector]
    //public Dictionary<string, string> TextReplaceKeyValueByCSV => CSVLanguageFileParser.GetLangDictionary(filePath, SelectedLanguage.value);

    [SerializeField, HideInInspector]
    public Dictionary<TextMeshProUGUI, string> startValues = new();

    private void Start()
    {
        SetTextsInUI();
    }

    private void OnApplicationQuit()
    {
        ResetStartValuesInTextMeshes();
    }

    private void OnDestroy()
    {
        ResetStartValuesInTextMeshes();
    }

    private void SaveStartValues()
    {
        startValues = new();

        /*if (TextReplaceKeyValueByCSV == null)
        {
            Debug.LogWarning("LANGUAGE FILE NOT FOUND OR FAULTY!");
            Debug.LogWarning("Path to language file set in Editor?");
            Debug.LogWarning("Is file in Resource folder?");

            return;
        }
        */

        foreach (var textMesh in uiTextMeshes)
        {
            if (textMesh == null) return;

            if (startValues.Keys.Contains(textMesh)) continue;
            startValues.Add(textMesh, textMesh.text);

            //if (TextReplaceKeyValueByCSV.ContainsKey(textMesh.text))
            {
                
            }
        }
    }

    public void SetTextsInUI()
    {
        if (startValues.Count == 0)
        {
            SaveStartValues();
        }

        if(SelectedLanguage.value == LanguageType.None)
        {
            foreach(var text in uiTextMeshes)
            {
                if (!startValues.ContainsKey(text)) continue;
                text.text = startValues[text];
            }
            return;
        }

        var dict = CSVLanguageFileParser.GetLangDictionary(filePath, SelectedLanguage.value);
        if (dict == null) return;

        foreach (var textMesh in startValues.Keys)
        {
            var startText = startValues[textMesh];
            if (dict.Keys.Contains(startText))
            {
                textMesh.text = dict[startText];
            }

            /*foreach (var startWord in TextReplaceKeyValueByCSV.Keys)
            {
                if (startValues[textMesh] == startWord)
                {
                    textMesh.text = TextReplaceKeyValueByCSV[startWord];
                }
            }
            */
        }
    }

    private void ResetStartValuesInTextMeshes()
    {
        foreach (var key in startValues.Keys)
        {
            key.text = startValues[key];
        }
    }
}
