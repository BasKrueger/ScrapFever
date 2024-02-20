using UnityEngine;

public class PoisonProjectile : AbstractProjectile
{
    [HideInInspector]
    public float puddleScale;
    [HideInInspector]
    public float puddleDamageRate;
    [HideInInspector]
    public float puddleDuration;

    public override void Destroy()
    {
        Pool.Return<PoisonProjectile>(this.gameObject);
    }

    protected override void Hit(AbstractEnemy enemy)
    {
    }

    protected override void LaunchEnd()
    {
        var puddle = Pool.Get("PoisonPuddle").GetComponent<PoisonPuddle>();
        puddle.damage = base.damage;
        puddle.cooldown = 1 / puddleDamageRate;

        var ray = new Ray(transform.position, Vector3.down * 10);
        var hit = new RaycastHit();
        int layer = 1 << LayerMask.NameToLayer("Ground");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
        {
            puddle.transform.position = hit.point + new Vector3(0, 0.5f, 0);
        }
        else
        {
            puddle.transform.position = transform.position;
        }

        puddle.SetUp(puddleDuration, puddleScale, base.source);

        base.LaunchEnd();

        Destroy();
    }
}
