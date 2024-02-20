using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : CustomUIElement
{
    [SerializeField]
    private OptionsScreenNew settings;
    [SerializeField]
    private CustomUIElement achivements;
    [SerializeField]
    private ConfirmationScreens mainMenuConfirmation;

    protected override void ShowStarted()
    {
        base.ShowStarted();
        if (Time.timeScale == 0) return;

        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    protected override void HideStarted()
    {
        base.HideStarted();
        Time.timeScale = 1;
    }

    public void ContinueClicked() => Hide();
    public void SettingsClicked() => settings.Show();
    public void AchivementsClick() => achivements.Show();
    public void RestartClicked() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void MainMenuClicked()
    {
        mainMenuConfirmation.Show();
        mainMenuConfirmation.confirmed += MainMenuConfirmed;
        AchivementManager.SaveAchivements();
    }

    public void MainMenuConfirmed()
    {
        FadeScreen.FadeInCompleted += LoadMainMenu;
        FadeScreen.FadeIn();
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void SetUp()
    {
        GetComponentInChildren<OptionsScreenNew>(true).SetUp();
    }
}
