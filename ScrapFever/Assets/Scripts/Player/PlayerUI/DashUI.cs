using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class DashUI : MonoBehaviour
{
    [SerializeField, Required, ShowInInspector]
    private Image fill;
    [SerializeField]
    private Animator shining;
    [SerializeField]
    private Animator blinkAnim;

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Blink()
    {
        blinkAnim.Play("Blink");
        shining.SetBool("Glow", false);
    }

    public void SetPercent(float percent)
    {
        fill.fillAmount = 1 - percent;
    }
}
