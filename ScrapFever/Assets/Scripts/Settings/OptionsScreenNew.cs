using System;

public enum OptionScreen
{
    Audio,
    Video,
    Game,
    Controls
}

public class OptionsScreenNew : CustomUIElement
{
    private new AudioSettings audio;
    private VideoSettings video;
    private GameSettings game;
    private ControlScreen controls;

    public void SetUp()
    {
        audio = GetComponentInChildren<AudioSettings>(true);
        video = GetComponentInChildren<VideoSettings>(true);
        game = GetComponentInChildren<GameSettings>(true);
        controls = GetComponentInChildren<ControlScreen>(true);

        audio.SetUp();
        video.SetUp();
        game.SetUp();
    }

    private void OnEnable()
    {
        GetComponentInChildren<SettingsNavigation>().clicked += ShowScreen;
    }

    private void OnDisable()
    {
        GetComponentInChildren<SettingsNavigation>().clicked -= ShowScreen;
    }

    protected override void ShowStarted()
    {
        base.ShowStarted();
        audio.Hide();
        video.Hide();
        controls.Hide();
    }

    public void ShowScreen(OptionScreen option)
    {
        if(option != OptionScreen.Audio) audio.Hide();
        if(option != OptionScreen.Video) video.Hide();
        if(option != OptionScreen.Game) game.Hide();
        if(option != OptionScreen.Controls) controls.Hide();

        switch (option)
        {
            case OptionScreen.Audio:
                audio.Show();
                break;
            case OptionScreen.Video:
                video.Show();
                break;
            case OptionScreen.Game:
                game.Show();
                break;
            case OptionScreen.Controls:
                controls.Show();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
