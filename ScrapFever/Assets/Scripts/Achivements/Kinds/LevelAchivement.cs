using SurvivorDTO;
using System.Collections.Generic;

public class LevelAchivement : AbstractAchivement
{
    public const string MAXLEVELACHIVEMENT = "GlueMaxAchivement";

    public LevelAchivement(int amount, AchivementDifficulty difficulty, List<SaveableAchievement> allAchivementsDatas)
         : base($"{MAXLEVELACHIVEMENT}{AchivementManager.SEPERATOR}{amount}{AchivementManager.SEPERATOR}{amount}"
             ,difficulty, allAchivementsDatas)
    {
        base.maxProgress = amount;
    }

    public override string GetName()
    {
        var name = CSVLanguageFileParser.GetLangDictionary("Translation/MenuText/Achivements", SelectedLanguage.value)["ObjectiveMaxLevel_Name"];
        name = name.Replace("<m1>", CSVLanguageFileParser.GetLangDictionary("Translation/MenuText/Achivements", SelectedLanguage.value)[difficulty.ToString()]);

        return name;
    }

    public override string GetDescription()
    {
        var description = CSVLanguageFileParser.GetLangDictionary("Translation/MenuText/Achivements", SelectedLanguage.value)["ObjectiveMaxLevel_Description"];
        description = description.Replace("<m1>", base.maxProgress.ToString());
        description = description.Replace("<m2>", (maxProgress - progress).ToString());

        return description;
    }

    protected override bool CanProgress(int value, object[] args)
    {
        return true;
    }
}
