using UnityEngine;

public class PauseButton : MonoBehaviour
{
    public void Clicked()
    {
        FindObjectOfType<PauseScreen>(true).Show();
    }
}
