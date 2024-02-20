using System.Collections.Generic;
using UnityEngine;

public class BalistaProjectile : AbstractWeapon
{
    protected override void Fire(Transform target)
    {
        var knive = base.GetProjectile<Balista>("BalistaProjectile");
        knive.Shoot(target.transform.position - transform.position);
    }

    protected override List<string> GetSpecificUpgradeNames()
    {
        return new List<string>();
    }

    protected override void SpecificUpgrade(WeaponUpgrade upgrade)
    {
        
    }
}
