using SurvivorDTO;
using System.Collections.Generic;

public static class AchivementManager 
{
    private static List<AbstractAchivement> achivements;
    public const string SEPERATOR = "___";

    public static List<AbstractAchivement> GetAchivements()
    {
        if(achivements == null || achivements == new List<AbstractAchivement>())
        {
            PopulateAchivements(out achivements);
        }

        return achivements;
    }

    public static void ProgressAchivement<T>(int value, object[] args = null) where T : AbstractAchivement
    {
        if (achivements == null || achivements == new List<AbstractAchivement>())
        {
            PopulateAchivements(out achivements);
        }

        foreach (var achivement in achivements)
        {
            if (achivement is not T) continue;

            achivement.TryProgress(value, args);
        }
    }

    private static void PopulateAchivements(out List<AbstractAchivement> result)
    {
        GameSaveFile gsf = new();
        gsf.LoadGameFileBinary();

        var saveFiles = new List<SaveableAchievement>();
        foreach(var saveFile in gsf.Achievements)
        {
            saveFiles.Add(saveFile);
        }

        result = new List<AbstractAchivement>();

        result.AddRange(new List<AbstractAchivement>()
        {
            #region KillAchivements
            #region Easy
            new KillAchivement(saveFiles,"GlueEnemyA_V1", AchivementDifficulty.Glue_Easy, 250),
            new KillAchivement(saveFiles,"GlueEnemyA_V2", AchivementDifficulty.Glue_Easy, 100),
            //new KillAchivement(saveFiles,"GlueEnemyA_V3", AchivementDifficulty.Glue_Easy, 25),

            new KillAchivement(saveFiles,"GlueEnemyB_V1", AchivementDifficulty.Glue_Easy, 250),
            new KillAchivement(saveFiles,"GlueEnemyB_V2", AchivementDifficulty.Glue_Easy, 100),
            new KillAchivement(saveFiles,"GlueEnemyB_V3", AchivementDifficulty.Glue_Easy, 25),

            new KillAchivement(saveFiles, "GlueLateBoomer", AchivementDifficulty.Glue_Easy, 1),
            new KillAchivement(saveFiles, "GlueFireball", AchivementDifficulty.Glue_Easy, 3),
            new KillAchivement(saveFiles, "GlueLantern", AchivementDifficulty.Glue_Easy, 40),
            new KillAchivement(saveFiles, "GlueContainer", AchivementDifficulty.Glue_Easy, 8),
        #endregion

            #region Medium
            new KillAchivement(saveFiles,"GlueEnemyA_V1", AchivementDifficulty.Glue_Medium, 1000),
            new KillAchivement(saveFiles,"GlueEnemyA_V2", AchivementDifficulty.Glue_Medium, 500),
            //new KillAchivement(saveFiles,"GlueEnemyA_V3", AchivementDifficulty.Glue_Medium, 100),

            new KillAchivement(saveFiles,"GlueEnemyB_V1", AchivementDifficulty.Glue_Medium, 1000),
            new KillAchivement(saveFiles,"GlueEnemyB_V2", AchivementDifficulty.Glue_Medium, 500),
            new KillAchivement(saveFiles,"GlueEnemyB_V3", AchivementDifficulty.Glue_Medium, 100),

            new KillAchivement(saveFiles, "GlueLateBoomer", AchivementDifficulty.Glue_Medium, 5),
            new KillAchivement(saveFiles, "GlueFireball", AchivementDifficulty.Glue_Medium, 10),
            new KillAchivement(saveFiles, "GlueLantern", AchivementDifficulty.Glue_Medium, 100),
            new KillAchivement(saveFiles, "GlueContainer", AchivementDifficulty.Glue_Medium, 20),
        #endregion

            #region Hard
            new KillAchivement(saveFiles,"GlueEnemyA_V1", AchivementDifficulty.Glue_Hard, 5000),
            new KillAchivement(saveFiles,"GlueEnemyA_V2", AchivementDifficulty.Glue_Hard, 2500),
            //new KillAchivement(saveFiles,"GlueEnemyA_V3", AchivementDifficulty.Glue_Hard, 1000),

            new KillAchivement(saveFiles,"GlueEnemyB_V1", AchivementDifficulty.Glue_Hard, 5000),
            new KillAchivement(saveFiles,"GlueEnemyB_V2", AchivementDifficulty.Glue_Hard, 2500),
            new KillAchivement(saveFiles,"GlueEnemyB_V3", AchivementDifficulty.Glue_Hard, 1000),

            new KillAchivement(saveFiles, "GlueLateBoomer", AchivementDifficulty.Glue_Hard, 15),
            new KillAchivement(saveFiles, "GlueFireball", AchivementDifficulty.Glue_Hard, 30),
            new KillAchivement(saveFiles, "GlueLantern", AchivementDifficulty.Glue_Hard, 250),
            new KillAchivement(saveFiles, "GlueContainer", AchivementDifficulty.Glue_Medium, 50),
            #endregion
            #endregion

            #region DamageAchivements
            #region easy
            new DamageAchivement(saveFiles, "Ballista", AchivementDifficulty.Glue_Easy, 1000),
            new DamageAchivement(saveFiles,"Books", AchivementDifficulty.Glue_Easy, 1000),
            new DamageAchivement(saveFiles,"Catapult", AchivementDifficulty.Glue_Easy, 1000),
            new DamageAchivement(saveFiles,"HeatRadiation", AchivementDifficulty.Glue_Easy, 1000),
            new DamageAchivement(saveFiles,"Knives", AchivementDifficulty.Glue_Easy, 1000),
            new DamageAchivement(saveFiles,"Poison", AchivementDifficulty.Glue_Easy, 1000),
            new DamageAchivement(saveFiles,"ScatterBomb", AchivementDifficulty.Glue_Easy, 1000),
            new DamageAchivement(saveFiles,"Scythe", AchivementDifficulty.Glue_Easy, 1000),
            new DamageAchivement(saveFiles,"Shotgun", AchivementDifficulty.Glue_Easy, 1000),
            new DamageAchivement(saveFiles,"Tesla", AchivementDifficulty.Glue_Easy, 1000),
            #endregion

            #region Medium
            new DamageAchivement(saveFiles,"Ballista", AchivementDifficulty.Glue_Medium, 10000),
            new DamageAchivement(saveFiles,"Books", AchivementDifficulty.Glue_Medium, 10000),
            new DamageAchivement(saveFiles,"Catapult", AchivementDifficulty.Glue_Medium, 10000),
            new DamageAchivement(saveFiles,"HeatRadiation", AchivementDifficulty.Glue_Medium, 10000),
            new DamageAchivement(saveFiles,"Knives", AchivementDifficulty.Glue_Medium, 10000),
            new DamageAchivement(saveFiles,"Poison", AchivementDifficulty.Glue_Medium, 10000),
            new DamageAchivement(saveFiles,"ScatterBomb", AchivementDifficulty.Glue_Medium, 10000),
            new DamageAchivement(saveFiles,"Scythe", AchivementDifficulty.Glue_Medium, 10000),
            new DamageAchivement(saveFiles,"Shotgun", AchivementDifficulty.Glue_Medium, 10000),
            new DamageAchivement(saveFiles,"Tesla", AchivementDifficulty.Glue_Medium, 10000),
            #endregion

            #region Hard
            new DamageAchivement(saveFiles,"Ballista", AchivementDifficulty.Glue_Hard, 30000),
            new DamageAchivement(saveFiles,"Books", AchivementDifficulty.Glue_Hard, 30000),
            new DamageAchivement(saveFiles,"Catapult", AchivementDifficulty.Glue_Hard, 30000),
            new DamageAchivement(saveFiles,"HeatRadiation", AchivementDifficulty.Glue_Hard, 30000),
            new DamageAchivement(saveFiles,"Knives", AchivementDifficulty.Glue_Hard, 30000),
            new DamageAchivement(saveFiles,"Poison", AchivementDifficulty.Glue_Hard, 30000),
            new DamageAchivement(saveFiles,"ScatterBomb", AchivementDifficulty.Glue_Hard, 30000),
            new DamageAchivement(saveFiles,"Scythe", AchivementDifficulty.Glue_Hard, 30000),
            new DamageAchivement(saveFiles,"Shotgun", AchivementDifficulty.Glue_Hard, 30000),
            new DamageAchivement(saveFiles,"Tesla", AchivementDifficulty.Glue_Hard, 30000),
            #endregion

            #endregion

            new TotalDamageAchivement(100000, AchivementDifficulty.Glue_Easy, saveFiles),
            new TotalDamageAchivement(100000000, AchivementDifficulty.Glue_Medium, saveFiles),
            new TotalDamageAchivement(1000000000, AchivementDifficulty.Glue_Easy, saveFiles),

            new DamageSurvivedAchievement(100, AchivementDifficulty.Glue_Easy, saveFiles),
            new DamageSurvivedAchievement(500, AchivementDifficulty.Glue_Medium, saveFiles),
            new DamageSurvivedAchievement(2500, AchivementDifficulty.Glue_Hard, saveFiles),

            new TimeAchievement(60*10, AchivementDifficulty.Glue_Easy, saveFiles),
            new TimeAchievement(60*25, AchivementDifficulty.Glue_Medium, saveFiles),
            new TimeAchievement(60*60, AchivementDifficulty.Glue_Hard, saveFiles),

            new LevelAchivement(20, AchivementDifficulty.Glue_Easy, saveFiles),
            new LevelAchivement(50, AchivementDifficulty.Glue_Medium, saveFiles),
            new LevelAchivement(100, AchivementDifficulty.Glue_Hard, saveFiles),
        });;
}

    public static void SaveAchivements()
    {
        List<SaveableAchievement> saveableAchievements = new();

        foreach (var achievement in GetAchivements())
        {
            saveableAchievements.Add(achievement.GetSaveFile());
        }

        SaveFileUtils.SaveGameFileBinary(saveableAchievements);
    }
}
