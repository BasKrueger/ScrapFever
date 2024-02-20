using Sirenix.OdinInspector;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField, FoldoutGroup("References")]
    private TextMeshProUGUI gameTimeDisplay;
    [SerializeField, FoldoutGroup("References")]
    private TextMeshProUGUI killCountDisplay;
    [SerializeField, FoldoutGroup("References")]
    private TextMeshProUGUI crystalCollectedDisplay;
    [SerializeField, FoldoutGroup("References")]
    private TextMeshProUGUI damageDoneDisplay;
    [SerializeField, FoldoutGroup("References")]
    private TextMeshProUGUI damageTakenDisplay;
    [SerializeField, FoldoutGroup("References")]
    private AchivementCard cardTemplate;
    [SerializeField, FoldoutGroup("References")]
    private Transform recentlyCompletedAchivementContent;
    [SerializeField, FoldoutGroup("References")]
    private TextMeshProUGUI noAchievementsCompleted;

    [SerializeField,FilePath(ParentFolder = "Assets/Resources", Extensions = ".csv", IncludeFileExtension = false, RequireExistingPath = true)]
    private string filePath;

    public void Show()
    {
        if (this.gameObject.activeInHierarchy) return;

        gameObject.SetActive(true);
        Time.timeScale = 0;

        UpdateDescriptions();
        UpdateRecentlyCompletedAchievments();

        AchivementManager.SaveAchivements();

        StartCoroutine(DelayedButtonShow());
    }

    private void UpdateDescriptions()
    {
        var rawTime = FindObjectOfType<Timer>(true).GetSurvivedTimeFormated();
        var rawKillCount = Pool.GetReturnedToPoolCount<AbstractEnemy>();
        var rawCrystalCount = Pool.GetReturnedToPoolCount<XPItem>();
        var rawDamageDealt = -1;
        var rawDamageTaken = -1;

        var dict = CSVLanguageFileParser.GetLangDictionary(filePath, SelectedLanguage.value);

        var translatedTime = dict["GlueGameTime"];
        translatedTime = translatedTime.Replace("<m1>", rawTime);

        var translatedKillCount = dict["GlueKillCount"];
        translatedKillCount = translatedKillCount.Replace("<m1>", rawKillCount.ToString());

        var translatedCollectedCrystals = dict["GlueKrystalCount"];
        translatedCollectedCrystals = translatedCollectedCrystals.Replace("<m1>", rawCrystalCount.ToString());

        var translatedDamageDealt = dict["GlueDamageDone"];
        translatedDamageDealt = translatedDamageDealt.Replace("<m1>", rawDamageDealt.ToString());

        var translatedDamageTaken = dict["GlueDamageTaken"];
        translatedDamageTaken = translatedDamageTaken.Replace("<m1>", rawDamageTaken.ToString());

        gameTimeDisplay.text = translatedTime;
        killCountDisplay.text = translatedKillCount;
        crystalCollectedDisplay.text = translatedCollectedCrystals;
        damageDoneDisplay.text = translatedDamageDealt;
        damageTakenDisplay.text = translatedDamageTaken;
    }

    private void UpdateRecentlyCompletedAchievments()
    {
        foreach(Transform t in recentlyCompletedAchivementContent)
        {
            Destroy(t.gameObject);
        }

        var completed = false;

        foreach(var achievement in AchivementManager.GetAchivements())
        {
            if (achievement.recentlyCompleted)
            {
                completed = true;

                achievement.recentlyCompleted = false;

                var card = Instantiate(cardTemplate);
                card.transform.SetParent(recentlyCompletedAchivementContent.transform, false);

                card.SetUp(achievement);
            }
        }

        noAchievementsCompleted.gameObject.SetActive(!completed);
    }

    public void MainMenuClicked()
    {
        FadeScreen.FadeInCompleted += MainMenu;
        FadeScreen.FadeIn();
    }

    public void RestartClicked()
    {
        FadeScreen.FadeInCompleted += Restart;
        FadeScreen.FadeIn();
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator DelayedButtonShow()
    {
        yield return new WaitForSecondsRealtime(0.4f);
        foreach(var card in recentlyCompletedAchivementContent.GetComponentsInChildren<AchivementCard>())
        {
            card.Show();
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
}
