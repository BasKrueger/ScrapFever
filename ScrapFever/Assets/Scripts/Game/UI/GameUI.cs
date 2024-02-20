using UnityEngine;

public class GameUI : MonoBehaviour
{
    private static GameUI instance;

    private PauseScreen pauseScreen;

    [SerializeField]
    private GameOverScreen gameOverScreen;
    [SerializeField]
    private GameOverScreen victoryScreen;

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        if (instance != null) Destroy(this);
        instance = this;

        pauseScreen = GetComponentInChildren<PauseScreen>(true);

        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
        playerInputActions.Navigation.Enable();
        playerInputActions.Navigation.Pause.Enable();
    }

    private void Start()
    {
        pauseScreen.SetUp();
    }

    private void Update()
    {
        if (playerInputActions.Navigation.Pause.WasPerformedThisFrame())
        {
            PauseShowHide();
        }
    }

    public void PauseShowHide()
    {
        if (!pauseScreen.gameObject.activeInHierarchy)
        {
            pauseScreen.Show();
        }
        else
        {
            pauseScreen.Hide();
        }
    }

    public static void ShowGameOverScreen() => instance.gameOverScreen.Show();
    public static void ShowVictoryScreen() => instance.victoryScreen.Show();
}
