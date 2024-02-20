using SurvivorDTO;
using System.Collections.Generic;

public class TotalDamageAchivement : AbstractAchivement
{
    public const string TIMEACHIEVEMENTNAME = "ObjectiveTotalDamage";

    public TotalDamageAchivement(int damage, AchivementDifficulty difficulty, List<SaveableAchievement> allDatas)
        : base($"{TIMEACHIEVEMENTNAME}{AchivementManager.SEPERATOR}{damage}{AchivementManager.SEPERATOR}{difficulty}", difficulty, allDatas)
    {
        base.maxProgress = damage;
    }

    public override string GetDescription()
    {
        var name = CSVLanguageFileParser.GetLangDictionary("Translation/MenuText/Achivements", SelectedLanguage.value)["Objective_TotalDamage_Description"];
        name = name.Replace("<m1>", base.maxProgress.ToString());
        name = name.Replace("<m2>", (base.maxProgress - base.progress).ToString());

        return name;
    }

    public override string GetName()
    {
        var name = CSVLanguageFileParser.GetLangDictionary("Translation/MenuText/Achivements", SelectedLanguage.value)["Objective_TotalDamage_Name"];
        name = name.Replace("<m1>", CSVLanguageFileParser.GetLangDictionary("Translation/MenuText/Achivements", SelectedLanguage.value)[difficulty.ToString()]);

        return name;
    }

    protected override bool CanProgress(int value, object[] args)
    {
        return true;
    }
}
