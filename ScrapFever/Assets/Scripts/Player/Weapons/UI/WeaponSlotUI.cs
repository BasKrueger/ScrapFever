using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private AbstractWeapon showing;
    private WeaponTooltip tooltip;

    [SerializeField]
    private Image icon;
    [SerializeField]
    private Image upgradeProgress;

    private void Awake()
    {
        icon.GetComponent<CanvasGroup>().alpha = 0;
        tooltip = GetComponentInChildren<WeaponTooltip>(true);
    }

    public bool TryShow(AbstractWeapon toShow)
    {
        if (showing != null) return false;

        showing = toShow;
        icon.sprite = toShow.sprite;
        icon.GetComponent<CanvasGroup>().alpha = 1;
        GetComponent<Animator>().Play("WeaponSlotIntro");

        toShow.upgraded += UpgradeImage;
        return true;
    }

    public void Select() => GetComponent<Animator>().SetBool("Selected", true);

    public void DeSelect() => GetComponent<Animator>().SetBool("Selected", false);

    public bool IsShowing(AbstractWeapon compare) => compare == showing;

    public void RemoveWeapon()
    {
        GetComponent<Animator>().Play("WeaponSlotOutro");
        GetComponent<Animator>().SetBool("Selected", false);

        tooltip.Hide();
    }

    public void UpgradeImage(float percent)
    {
        upgradeProgress.fillAmount = percent;
    }

    public void AnimationRemoveTrigger()
    {
        showing = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (showing == null || tooltip == null) return;
        tooltip.Show(showing);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (showing == null || tooltip == null) return;
        tooltip.Hide();
    }
}
