using SurvivorDTO;
using System.Collections.Generic;

public class DamageAchivement : AbstractAchivement
{
    public const string DAMAGEACHIVEMENTNAME = "ObjectiveDamage";
    private string glueWeaponName;

    public DamageAchivement(List<SaveableAchievement> allAchivementsDatas, string glueWeaponName, AchivementDifficulty difficulty, int damageAmount) 
        : base($"{DAMAGEACHIVEMENTNAME}{AchivementManager.SEPERATOR}{glueWeaponName}{AchivementManager.SEPERATOR}{damageAmount}{AchivementManager.SEPERATOR}{difficulty}", 
            difficulty, allAchivementsDatas)
    {
        this.glueWeaponName = glueWeaponName;
        base.maxProgress = damageAmount;
    }

    public bool TryProgress(IDamageSource source, int progress)
    {
        if (source.glueSourceName == glueWeaponName)
        {
            base.Progress(progress);
            return true;
        }

        return false;
    }

    public override string GetName()
    {
        var name = CSVLanguageFileParser.GetLangDictionary("Translation/MenuText/Achivements", SelectedLanguage.value)["Objective_Damage_Name"];
        name = name.Replace("<m1>", CSVLanguageFileParser.GetLangDictionary("Translation/MenuText/Weapons", SelectedLanguage.value)[$"{glueWeaponName}_Name"]);
        name = name.Replace("<m2>", CSVLanguageFileParser.GetLangDictionary("Translation/MenuText/Achivements", SelectedLanguage.value)[difficulty.ToString()]);

        return name;
    }

    public override string GetDescription()
    {
        var description = CSVLanguageFileParser.GetLangDictionary("Translation/MenuText/Achivements", SelectedLanguage.value)["Objective_Damage_Description"];
        description = description.Replace("<m1>", base.maxProgress.ToString());
        description = description.Replace("<m2>", CSVLanguageFileParser.GetLangDictionary("Translation/MenuText/Weapons", SelectedLanguage.value)[$"{glueWeaponName}_Name"]);
        description = description.Replace("<m3>", (maxProgress - progress).ToString());

        return description;
    }

    protected override bool CanProgress(int value, object[] args)
    {
        foreach(var thing in args)
        {
            if(thing is IDamageSource && ((IDamageSource)thing).glueSourceName == this.glueWeaponName)
            {
                return true;
            }
        }

        return false;
    }
}
