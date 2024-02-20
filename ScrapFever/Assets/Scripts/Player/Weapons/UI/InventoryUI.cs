using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [field:SerializeField]
    public WeaponUnlockWindow unlockWindow { get; private set; }

    [field:SerializeField]
    public ActiveWeaponsUI activeWeapons { get; private set; }

    public void SetUp(int slots)
    {
        activeWeapons.Setup(slots);
    }
}
