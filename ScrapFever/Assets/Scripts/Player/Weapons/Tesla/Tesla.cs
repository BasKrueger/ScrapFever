using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class Tesla : AbstractWeapon
{
    [SerializeField, MinValue(0), TabGroup("Specifics")]
    private int bounces;

    protected override void Fire(Transform target)
    {
        var bullet = base.GetProjectile<Spark>("TeslaSpark");
        bullet.bounces = new IntStat(bounces);

        bullet.Shoot(target.transform.position - transform.position);
    }

    protected override List<string> GetSpecificUpgradeNames()
    {
        return new List<string>()
        {
            ("GlueBounces")
        };
    }

    protected override void SpecificUpgrade(WeaponUpgrade upgrade)
    {
        upgrade.TryUpgradeSpecific(ref bounces, "GlueBounces");
    }
}
