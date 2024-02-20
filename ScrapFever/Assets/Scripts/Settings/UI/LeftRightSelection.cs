using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LeftRightSelection : MonoBehaviour
{
    public event Action<float> ValueChanged;

    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    [FilePath(ParentFolder = "Assets/Resources", Extensions = ".csv", IncludeFileExtension = false, RequireExistingPath = true)]
    private string filePath;
    [SerializeField]
    private List<string> glueOptions;

    [HideInInspector]
    public int currentIndex;

    private void Awake()
    {
        SelectedLanguage.languageChanged += UpdateText;
        UpdateText();
    }

    public void SetUp(int index)
    {
        currentIndex = index;
        UpdateText();
    }

    public void Left()
    {
        currentIndex++;
        if (currentIndex > glueOptions.Count - 1)
        {
            currentIndex = 0;
        }
        if (currentIndex < 0)
        {
            currentIndex = glueOptions.Count - 1;
        }

        ValueChanged?.Invoke(currentIndex);
        UpdateText();
    }

    public void Right()
    {
        currentIndex--;
        if (currentIndex > glueOptions.Count - 1)
        {
            currentIndex = 0;
        }
        if (currentIndex < 0)
        {
            currentIndex = glueOptions.Count - 1;
        }

        ValueChanged?.Invoke(currentIndex);
        UpdateText();
    }

    public void UpdateText()
    {
        var dict = CSVLanguageFileParser.GetLangDictionary(filePath, SelectedLanguage.value);
        if (dict == null) return;
        if (glueOptions == null) return;
        if (glueOptions.Count - 1 < currentIndex) return;
        if (!dict.Keys.Contains(glueOptions[currentIndex])) return;

        text.text = dict[glueOptions[currentIndex]];
    }
}
