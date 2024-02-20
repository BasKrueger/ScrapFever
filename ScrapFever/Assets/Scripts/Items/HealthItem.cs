using Sirenix.OdinInspector;
using UnityEngine;

public class HealthItem : AbstractItem, IPoolable
{
    [Title("HealthItem")]
    [SerializeField]
    private int value;

    protected override void PickedUp(Player player)
    {
        player.RestoreHealth(value);
        Destroy(this.gameObject);
    }
}
