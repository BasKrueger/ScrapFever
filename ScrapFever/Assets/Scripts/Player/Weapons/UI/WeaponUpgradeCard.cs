using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class WeaponUpgradeCard : MonoBehaviour
{
    public event Action<WeaponUpgradeCard> selected;

    [SerializeField, FoldoutGroup("References")]
    private Image Icon;

    [SerializeField, FoldoutGroup("References")]
    private TextMeshProUGUI weaponName;

    [SerializeField, FoldoutGroup("References")]
    private TextMeshProUGUI upgradeDescription;

    [SerializeField, FoldoutGroup("References")]
    private VideoPlayer player;

    public void ShowNewWeapon(AbstractWeapon weapon)
    {
        Icon.sprite = weapon.sprite;
        weaponName.text = weapon.Name;
        upgradeDescription.text = weapon.Description;

        gameObject.SetActive(true);

        if (Application.isMobilePlatform)
        {
            player.enabled = false;
        }

        if(player != null)
            player.clip = weapon.clip;
    }

    public void ShowUpgrade(AbstractWeapon weapon)
    {
        Icon.sprite = weapon.sprite;
        weaponName.text = weapon.Name;
        upgradeDescription.text = weapon.GetNextUpgradeDescription();

        gameObject.SetActive(true);

        if (Application.isMobilePlatform)
        {
            player.enabled = false;
        }

        if (player != null)
            player.clip = weapon.clip;
    }

    public void Hide()
    {
        gameObject.SetActive(false);

        player.Stop();
    }

    public void OnClick()
    {
        selected?.Invoke(this);
    }
}
