using System;

public class ConfirmationScreens : CustomUIElement
{
    public event Action confirmed;
    public event Action denied;

    public void Confirm()
    {
        Hide();
        confirmed?.Invoke();
    }

    public void Denie()
    {
        Hide();
        denied?.Invoke();
    }
}
