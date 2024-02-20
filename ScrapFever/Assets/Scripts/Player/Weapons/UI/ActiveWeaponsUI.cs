using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class ActiveWeaponsUI : MonoBehaviour
{
    [SerializeField, AssetsOnly, FoldoutGroup("References")]
    private WeaponSlotUI slotTemplate;

    [SerializeField, ChildGameObjectsOnly, FoldoutGroup("References")]
    private Transform weaponSlotHolder;

    private void Awake()
    {
        foreach(var weapon in GetComponentsInChildren<WeaponSlotUI>())
        {
            Destroy(weapon.gameObject);
        }
    }

    public void Setup(int slots)
    {
        for (int i = 0; i < slots; i++)
        {
            var slot = Instantiate(slotTemplate);
            slot.transform.SetParent(weaponSlotHolder, false);
        }
    }

    public void AddWeapon(AbstractWeapon toShow, int slot)
    {
        if (!this.gameObject.activeInHierarchy) return;

        StartCoroutine(AddWeaponOncePossible(toShow, slot));
    }

    public void RemoveWeapon(AbstractWeapon toRemove)
    {
        foreach (var slot in weaponSlotHolder.transform.GetComponentsInChildren<WeaponSlotUI>())
        {
            if (slot.IsShowing(toRemove))
            {
                slot.RemoveWeapon();
            }
        }
    }

    private IEnumerator AddWeaponOncePossible(AbstractWeapon weapon, int slot)
    {
        yield return new WaitForSeconds(0.1f);

        var uiSlot = weaponSlotHolder.GetComponentsInChildren<WeaponSlotUI>()[slot];
        while (!uiSlot.TryShow(weapon))
        {
            yield return new WaitForSeconds(0.1f);
        }
    }
}
