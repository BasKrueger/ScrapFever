using System;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
    public static FadeScreen instance;

    public static event Action FadeInCompleted;
    public static event Action FadeOutCompleted;

    private Animator anim;
    
    [SerializeField]
    private Image blend;

    private void Awake()
    {
        instance = this;
        anim = GetComponent<Animator>();

        Time.timeScale = 1;
    }

    public static void FadeIn(float bonusSpeed = 0)
    {
        instance.anim.SetFloat("Speed", 1 + bonusSpeed);
        instance.anim.SetTrigger("FadeIn");
        Time.timeScale = 1;
    }

    public static void FadeIn(Color c, float bonusSpeed = 0)
    {
        instance.blend.color = c;
        FadeIn(bonusSpeed);
    }
    public static void FadeOut(Color c, float bonusSpeed = 0)
    {
        instance.blend.color = c;
        FadeOut(bonusSpeed);
    }

    public static void FadeOut(float bonusSpeed = 0)
    {
        instance.anim.SetFloat("Speed", 1 + bonusSpeed);
        instance.anim.SetTrigger("FadeOut");
        Time.timeScale = 1;
    }

    private void OnFadeInCompleted()
    {
        FadeInCompleted?.Invoke();
        FadeInCompleted = null;
    }
    private void OnFadeOutCompleted()
    {
        FadeOutCompleted?.Invoke();
        FadeOutCompleted = null;
    }
}
