using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponUnlockWindow : MonoBehaviour
{
    public event Action<UpgradeInformation> upgradeSelected;

    private void Awake()
    {
        foreach (var upgradeCard in GetComponentsInChildren<WeaponUpgradeCard>())
        {
            upgradeCard.selected += OnCardSelected;
        }
    }

    public void DisplayOffers(List<UpgradeInformation> infos)
    {
        EventSystem.current.SetSelectedGameObject(null);

        var upgradeCards = GetComponentsInChildren<WeaponUpgradeCard>();

        for (int i = 0; i< infos.Count && i < upgradeCards.Length; i++)
        {
            upgradeCards[i].Show(infos[i]);
        }

        Time.timeScale = 0;
        gameObject.SetActive(true);
    }


    private void OnCardSelected(UpgradeInformation selected)
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);

        upgradeSelected?.Invoke(selected);
    }
}
