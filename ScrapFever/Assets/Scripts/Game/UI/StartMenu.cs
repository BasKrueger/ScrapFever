using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : CustomUIElement
{
    [SerializeField]
    private ConfirmationScreens exitConfirmation;

    private PlayerInputActions input;
    private OptionsScreenNew options;

    private void Start()
    {
        base.Show();
        options = GetComponentInChildren<OptionsScreenNew>(true);
        options.SetUp();

        input = new PlayerInputActions();
        input.Enable();
        exitConfirmation.confirmed += ExitConfirmed;
    }

    public void StartGame()
    {
        FadeScreen.FadeInCompleted += OnFadeOutCompleted;
        FadeScreen.FadeIn();
    }

    public void ExitGame()
    {
        exitConfirmation.Show();
    }

    protected override void HideCompleted()
    {
        base.HideCompleted();
        if (this.gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (input.Navigation.Pause.WasPerformedThisFrame())
        {
            base.Hide();
        }
    }

    public void ExitConfirmed()
    {
        AchivementManager.SaveAchivements();
        Application.Quit();
    }

    private void OnFadeOutCompleted()
    {
        SceneManager.LoadScene(1);
    }
}
