using System.Collections.Generic;
using UnityEngine;

public class Knive : AbstractWeapon
{
    protected override void Fire(Transform target)
    {
        var knive = base.GetProjectile<KniveProjectile>("KniveProjectile");
        knive.Shoot(target.position - transform.position);
    }

    protected override List<string> GetSpecificUpgradeNames()
    {
        return new List<string>();
    }

    protected override void SpecificUpgrade(WeaponUpgrade upgrade)
    {
        
    }
}
