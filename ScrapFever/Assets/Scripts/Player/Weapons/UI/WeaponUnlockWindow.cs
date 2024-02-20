using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponUnlockWindow : MonoBehaviour
{
    public event Action<int> upgradeSelected;
    private List<WeaponUpgradeCard> upgradeCards;

    public void DisplayUpgrades(List<(AbstractWeapon weapon, bool isUpgrade)> weapons)
    {
        EventSystem.current.SetSelectedGameObject(null);

        upgradeCards = new List<WeaponUpgradeCard>();
        foreach (var upgradeCard in GetComponentsInChildren<WeaponUpgradeCard>())
        {
            upgradeCards.Add(upgradeCard);
            upgradeCard.selected += OnCardSelected;
        }

        for (int i = 0;i < upgradeCards.Count; i++)
        {
            if(i < weapons.Count)
            {
                if (weapons[i].isUpgrade)
                {
                    upgradeCards[i].ShowUpgrade(weapons[i].weapon);
                }
                else
                {
                    upgradeCards[i].ShowNewWeapon(weapons[i].weapon);
                }
            }
            else
            {
                upgradeCards[i].Hide();
            }
        }

        Time.timeScale = 0;
        gameObject.SetActive(true);
    }

    private void OnCardSelected(WeaponUpgradeCard selected)
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);

        upgradeSelected?.Invoke(upgradeCards.IndexOf(selected));
    }
}
