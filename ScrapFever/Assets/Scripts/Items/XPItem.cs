using Sirenix.OdinInspector;
using UnityEngine;

public class XPItem : AbstractItem
{
    const int MAXXPITEMS = 125;

    [Title("XpItem")]
    [SerializeField]
    private int value;
    [SerializeField]
    private float movementSpeed;

    private TrailRenderer trail;

    public bool IsOnScreen => gameObject.IsOnScreen(60.0f);

    private void Start()
    {
        transform.rotation = Quaternion.Euler(0, Random.rotation.eulerAngles.y, 0);
    }

    public void IncreaseValue(int value)
    {
        this.value += Mathf.Clamp(value, 0, 1000);
    }

    protected override void PickedUp(Player player)
    {
        player.EarnXP(value);

        if (trail != null)
        {
            trail.enabled = false;
        }

        Pool.Return<XPItem>(this.gameObject);
    }

    protected override void InternalOnReturnedToPool()
    {
        if (trail == null)
        {
            trail = GetComponentInChildren<TrailRenderer>();
        }
    }

    protected override void InternalOnTakenfromPool()
    {
        if(Pool.GetOutOfPoolCount<XPItem>() > MAXXPITEMS)
        {
            Pool.Return<XPItem>(this.gameObject);
            return;
        }

        if (trail == null)
        {
            trail = GetComponentInChildren<TrailRenderer>();
        }
        trail.enabled = true;
    }
}
