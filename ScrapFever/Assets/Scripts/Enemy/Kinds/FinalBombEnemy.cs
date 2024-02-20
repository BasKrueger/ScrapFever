using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalBombEnemy : AbstractEnemy
{
    protected override void Die()
    {
        FadeScreen.FadeIn(Color.white, 0.5f);
        FadeScreen.FadeInCompleted += () => SceneManager.LoadScene(2);
        base.Die();
    }
}
