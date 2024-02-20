using SurvivorDTO;
using System.Collections.Generic;

public class KillAchivement : AbstractAchivement
{
    public const string KILLACHIVEMENTNAME = "ObjectiveKill";
    private string glueTargetName;

    public KillAchivement(List<SaveableAchievement> allAchivementsDatas, string glueTargetName, AchivementDifficulty difficulty, int killAmount) 
        : base($"{KILLACHIVEMENTNAME}{AchivementManager.SEPERATOR}{glueTargetName}{AchivementManager.SEPERATOR}{killAmount}{AchivementManager.SEPERATOR}{difficulty}"
            ,difficulty, allAchivementsDatas)
    {
        this.glueTargetName = glueTargetName;
        base.maxProgress = killAmount;
    }

    public bool TryProgress(AbstractEnemy target, int progress)
    {
        if(target.glueName == glueTargetName)
        {
            base.Progress(progress);
            return true;
        }

        return false;
    }

    public override string GetName()
    {
        var name = CSVLanguageFileParser.GetLangDictionary("Translation/MenuText/Achivements", SelectedLanguage.value)["Objective_Kill_Name"];
        name = name.Replace("<m1>", CSVLanguageFileParser.GetLangDictionary("Translation/MenuText/Enemies", SelectedLanguage.value)[$"{glueTargetName}_Name"]);
        name = name.Replace("<m2>", CSVLanguageFileParser.GetLangDictionary("Translation/MenuText/Achivements", SelectedLanguage.value)[difficulty.ToString()]);

        return name;
    }

    public override string GetDescription()
    {
        var description = CSVLanguageFileParser.GetLangDictionary("Translation/MenuText/Achivements", SelectedLanguage.value)["Objective_Kill_Description"];
        description = description.Replace("<m1>", base.maxProgress.ToString());
        description = description.Replace("<m2>", CSVLanguageFileParser.GetLangDictionary("Translation/MenuText/Enemies", SelectedLanguage.value)[$"{glueTargetName}_Name"]);
        description = description.Replace("<m3>", (maxProgress - progress).ToString());

        return description;
    }

    protected override bool CanProgress(int value, object[] args)
    {
        foreach (var thing in args)
        {
            if (thing is AbstractEnemy && ((AbstractEnemy)thing).glueName == this.glueTargetName)
            {
                return true;
            }
        }

        return false;
    }
}
