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
    private InventoryUI ui;
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
        ui.SetUp(abilitySlots);

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
        StartCoroutine(delay());

        IEnumerator delay()
        {
            yield return new WaitForEndOfFrame();
            List<(AbstractWeapon weapon, bool isUpgrade)> offers = GetLevelUpOffers();
            if (offers.Count <= 0) yield break;

            int selectedUpgrade = -1;
            ui.unlockWindow.upgradeSelected += (int value) => selectedUpgrade = value;
            ui.unlockWindow.DisplayUpgrades(offers);

            Time.timeScale = 0;
            while (selectedUpgrade < 0)
            {
                yield return new WaitForEndOfFrame();
            }
            Time.timeScale = 1;

            var selected = offers[selectedUpgrade];
            if (selected.isUpgrade)
            {
                selected.weapon.Upgrade();
            }
            else
            {
                AddWeapon(selected.weapon);
            }
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

            ui.activeWeapons.AddWeapon(weapon, activeWeapons.IndexOf(weapon));
            worldUI.Show(weapon);

            unObtainedWeapons.Remove(template);
        }
    }

    private List<(AbstractWeapon, bool)> GetLevelUpOffers()
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

        List<(AbstractWeapon weapon, bool isUpgrade)> offers = new List<(AbstractWeapon weapons, bool isUpgrade)>();
        for (int i = 0; i < 2 && allOptions.Count > 0; i++)
        {
            var weapon = allOptions.GetRandom();
            var isUpgrade = activeWeapons.Contains(weapon) && !weapon.IsMaxed();

            offers.Add((weapon, isUpgrade));
            allOptions.Remove(weapon);
        }

        return offers;
    }
}
