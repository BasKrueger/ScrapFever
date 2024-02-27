using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class PlayerWeapons
{
    public delegate Coroutine startCoroutine(IEnumerator routine);
    private startCoroutine StartCoroutine;

    #region Settings
    [SerializeField]
    private AbstractWeapon startingWeapon;
    [SerializeField]
    private int abilitySlots = 4;
    [SerializeField, FoldoutGroup("References")]
    private List<AbstractWeapon> unObtainedWeapons = new List<AbstractWeapon>();
    #endregion

    [FoldoutGroup("References"), SerializeField]
    private Transform weaponHolder;
    [FoldoutGroup("References"), SerializeField]
    private InventoryUI weaponUI;
    [FoldoutGroup("References"), SerializeField]
    private WeaponUnlockCanvas worldUI;

    private AbstractWeapon[] activeWeapons;
    
    public void AwakeSetUp(startCoroutine start)
    {
        this.StartCoroutine = start;
        activeWeapons = new AbstractWeapon[abilitySlots];
    }

    public void SetUp()
    {
        weaponUI.SetUp(abilitySlots);

        if(startingWeapon == null)
        {
            FadeScreen.FadeOutCompleted += () => { LevelUp(); };
        }
        else
        {
            FadeScreen.FadeOutCompleted += () => AddWeapon(startingWeapon);
        }
    }

    public void LevelUp()
    {
        var offers = GetLevelUpOffers();
        if (offers.Count <= 0) return;

        weaponUI.unlockWindow.upgradeSelected += OnUpgradeSelected;
        weaponUI.unlockWindow.DisplayOffers(offers);
    }

    private void OnUpgradeSelected(UpgradeInformation info)
    {
        weaponUI.unlockWindow.upgradeSelected -= OnUpgradeSelected;

        if (info.isUpgrade)
        {
            info.weapon.Upgrade();
        }
        else
        {
            AddWeapon(info.weapon);
        }
    }

    public bool IsMaxed()
    {
        if (activeWeapons.Contains(null)) return false;

        foreach(var weapon in activeWeapons)
        {
            if (!weapon.IsMaxed())
            {
                return false;
            }
        }

        return true;
    }

    private void AddWeapon(AbstractWeapon template)
    {
        if (activeWeapons.Contains(null))
        {
            var weapon = GameObject.Instantiate(template);
            weapon.transform.SetParent(weaponHolder);
            weapon.transform.position = weaponHolder.position;

            activeWeapons.TryReplaceFirst(null, weapon);

            weaponUI.activeWeapons.AddWeapon(weapon, activeWeapons.IndexOf(weapon));
            worldUI.Show(weapon);

            unObtainedWeapons.Remove(template);
        }
    }

    private List<UpgradeInformation> GetLevelUpOffers()
    {
        var allOptions = new List<AbstractWeapon>();
        
        foreach(var weapon in activeWeapons)
        {
            if (weapon == null) continue;
            if (weapon.IsMaxed()) continue;

            allOptions.Add(weapon);
        }

        if (activeWeapons.Contains(null))
        {
            allOptions.AddRange(unObtainedWeapons);
        }

        List<UpgradeInformation> offers = new List<UpgradeInformation>();
        for (int i = 0; i < 2 && allOptions.Count > 0; i++)
        {
            var upgradeOption = new UpgradeInformation();
            upgradeOption.weapon = allOptions.GetRandom();
            upgradeOption.isUpgrade = activeWeapons.Contains(upgradeOption.weapon) && !upgradeOption.weapon.IsMaxed();

            offers.Add(upgradeOption);
            allOptions.Remove(upgradeOption.weapon);
        }

        return offers;
    }
}

public struct UpgradeInformation
{
    public AbstractWeapon weapon;
    public bool isUpgrade;
}
