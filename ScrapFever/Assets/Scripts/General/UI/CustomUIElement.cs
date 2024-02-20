using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CustomUIElement : MonoBehaviour
{
    [Header("UI Element")]

    [SerializeField]
    protected List<CustomUIElement> closeBeforeThis;
    
    [SerializeField]
    private List<CustomUIElement> showWithThis;
    [SerializeField, ShowIf("ShowGroupShownDelay")]
    private float groupShowDelay = 0.15f;
    private bool ShowGroupShownDelay() => showWithThis != null && showWithThis.Count > 0;


    [SerializeField]
    private List<CustomUIElement> hideWithThis;
    [SerializeField, ShowIf("ShowGroupHideDelay")]
    private float groupHideDelay = 0.15f;
    private bool ShowGroupHideDelay() => hideWithThis != null && hideWithThis.Count > 0;

    [SerializeField]
    private bool canBeHidden = true;

    [SerializeField, DisableIf("animated")]
    private bool fading;
    [SerializeField, ShowIf("fading")]
    private float fadeDuration = 0.15f;
   

    [SerializeField, DisableIf("fading")]
    private bool animated;
    [SerializeField, ShowIf("animated")]
    private Animator anim;
    [SerializeField, ShowIf("animated")]
    private string animShowBool;

    public void Show()
    {
        if (this.gameObject == null) return;

        gameObject.SetActive(true);

        StartCoroutine(ShowLinked());

        if (fading)
        {
            StartCoroutine(FadeIn());
        }
        else if (animated)
        {
            anim.SetBool(animShowBool, true);
        }
        else
        {
            ShowStarted();
            ShowCompleted();
        }
    }

    protected virtual void ShowStarted() { }
    protected virtual void ShowCompleted() { }

    public void Hide()
    {
        if (!this.gameObject.activeInHierarchy) return;

        foreach (var element in closeBeforeThis)
        {
            if (element == null) continue;
            if (element.gameObject.activeInHierarchy)
            {
                element.Hide();
                return;
            }
        }

        if (!canBeHidden) return;

        StartCoroutine(HideLinked());

        if (fading)
        {
            StartCoroutine(FadeOut());
        }
        else if (animated)
        {
            anim.SetBool(animShowBool, false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    protected virtual void HideStarted() { gameObject.SetActive(true); }
    protected virtual void HideCompleted() { gameObject.SetActive(false); }

    IEnumerator FadeIn()
    {
        ShowStarted();

        var group = GetComponent<CanvasGroup>();

        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += 0.01f;
            group.alpha = timer / fadeDuration;
            yield return new WaitForSecondsRealtime(0.01f);
        }

        ShowCompleted();
    }

    IEnumerator ShowLinked()
    {
        foreach (var element in showWithThis)
        {
            yield return new WaitForSecondsRealtime(groupShowDelay);
            if (element == null) continue;
            element.Show();
        }
    }

    IEnumerator FadeOut()
    {
        HideStarted();

        var group = GetComponent<CanvasGroup>();

        float timer = fadeDuration;
        while (timer > 0)
        {
            timer -= 0.01f;
            group.alpha = timer / fadeDuration;
            yield return new WaitForSecondsRealtime(0.01f);
        }

        gameObject.SetActive(false);
        HideCompleted();
    }

    IEnumerator HideLinked()
    {
        foreach (var element in hideWithThis)
        {
            if (element == null) continue;
            yield return new WaitForSecondsRealtime(groupHideDelay);
            element.Hide();
        }
    }
}
