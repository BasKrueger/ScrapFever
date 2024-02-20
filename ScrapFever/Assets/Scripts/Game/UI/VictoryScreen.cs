using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{
    public void Show()
    {
        gameObject.SetActive(true);
        AchivementManager.SaveAchivements();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
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
}
