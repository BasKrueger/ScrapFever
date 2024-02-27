using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponUpgrade
{
    [SerializeField, LabelText("Upgrade")]
    private UpgradeWrapper content = new UpgradeWrapper();

    public AbstractWeapon.WeaponTargets newTarget => content.newTarget;
    public int bonusDamage => content.bonusDamage;
    public float bonusFireRate => content.bonusFireRate;

    public void SetUp(List<string> content) => this.content.SetUp(content);
    public bool TryUpgradeSpecific(ref int value, string name) => content.TryUpgradeSpecific(ref value, name);
    public bool TryUpgradeSpecific(ref float value, string name) => content.TryUpgradeSpecific(ref value, name);
    public string GetDescription() => content.GetDescription();
}

[System.Serializable]
public class UpgradeWrapper
{
    private const string translationPath = "Translation/MenuText/Weapons";

    [field: SerializeField, EnumToggleButtons(), FoldoutGroup("Generic")]
    public AbstractWeapon.WeaponTargets newTarget { get; private set; }

    [field: SerializeField, FoldoutGroup("Generic")]
    public int bonusDamage { get; private set; }

    [field: SerializeField, SuffixLabel("shots per seconds", true), FoldoutGroup("Generic")]
    public float bonusFireRate { get; private set; }

    [SerializeField, LabelText("Specific"), ShowIf("HasSpecificUpgrades"), ListDrawerSettings(HideAddButton = true, HideRemoveButton = true)]
    private List<SpecificUpgrade> specificUpgrades;

    public void SetUp(List<string> content)
    {
        specificUpgrades = new List<SpecificUpgrade>();
        foreach (var c in content)
        {
            specificUpgrades.Add(new SpecificUpgrade(c));
        }
    }

    public bool TryUpgradeSpecific(ref int value, string name)
    {
        foreach (var upgrade in specificUpgrades)
        {
            if (upgrade.MatchesName(name))
            {
                value += upgrade;
                return true;
            }
        }

        return false;
    }

    public bool TryUpgradeSpecific(ref float value, string name)
    {
        foreach (var upgrade in specificUpgrades)
        {
            if (upgrade.MatchesName(name))
            {
                value += upgrade;
                return true;
            }
        }

        return false;
    }

    public string GetDescription()
    {
        string result = "";

        var sign = bonusDamage >= 0 ? "+" : "-";

        if (bonusDamage != 0) result += $"{CSVLanguageFileParser.GetLangDictionary(translationPath, SelectedLanguage.value)["GlueBonusDamage"]} : {sign}{Mathf.Abs(bonusDamage)}\n";
        if (bonusFireRate != 0) result += $"{CSVLanguageFileParser.GetLangDictionary(translationPath, SelectedLanguage.value)["GlueBonusFireRate"]} : {sign}{Mathf.Abs(bonusFireRate)}\n";

        foreach (var upgrade in specificUpgrades)
        {
            result += upgrade.GetDescription(translationPath);
        }

        return result;
    }

    //used by Odin
    private bool HasSpecificUpgrades() => specificUpgrades.Count > 0;
}

[System.Serializable]
public class SpecificUpgrade
{
    [SerializeField, HideInInspector]
    private string upgradeName = "";
    [SerializeField, LabelText("@upgradeName")]
    private float value;

    public bool MatchesName(string toCompare)
    {
        return upgradeName == toCompare;
    }

    public SpecificUpgrade(string toSet)
    {
        this.upgradeName = toSet;
    }

    public string GetDescription(string path)
    {
        if (value == 0) return "";

        var sign = value >= 0 ? "+" : "-";

        return $"{CSVLanguageFileParser.GetLangDictionary(path, SelectedLanguage.value)[upgradeName]} : {sign}{Mathf.Abs(value)}\n";
    }

    public static implicit operator float(SpecificUpgrade upgrade) => upgrade.value;

    public static implicit operator int(SpecificUpgrade upgrade) => Mathf.RoundToInt(upgrade.value);
}