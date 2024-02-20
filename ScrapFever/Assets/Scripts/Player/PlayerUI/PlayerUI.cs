using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField, FoldoutGroup("References")]
    private XPBar xpBar;
    [SerializeField, FoldoutGroup("References")]
    private TextMeshProUGUI playerLevel;

    [SerializeField]
    [FilePath(ParentFolder = "Assets/Resources", Extensions = ".csv", IncludeFileExtension = false, RequireExistingPath = true)]
    private string filePath;

    private void Awake()
    {
        xpBar = GetComponentInChildren<XPBar>();
    }

    public void SetXPBar(float current, float max) => xpBar.UpdateValue(current, max); 
    public void SetLevel(int value)
    {
        var text = CSVLanguageFileParser.GetLangDictionary(filePath, SelectedLanguage.value)["GlueLevel"];
        text = text.Replace("<m1>", value.ToString());
        playerLevel.text = text;
    }
    public void SetLevel(string value)
    {
        var text = CSVLanguageFileParser.GetLangDictionary(filePath, SelectedLanguage.value)["GlueLevel"];
        text = text.Replace("<m1>", value);
        playerLevel.text = text;
    }

    public void HideDashUI() { } //dashUI.Hide();
    public void ShowDashUI() { } //dashUI.Show();
    public void BlinkDashUI() { } //dashUI.Blink();
    public void SetDashCooldownPercent(float percent) { }
}
