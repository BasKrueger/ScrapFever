using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class ScatterBomb : AbstractWeapon
{
    [SerializeField, TabGroup("Specifics")]
    private float explosionRange;
    [SerializeField, TabGroup("Specifics")]
    private int scatterCount;
    [SerializeField, TabGroup("Specifics")]
    private float rangeUntilExplosion;

    protected override void Fire(Transform target)
    {
        var projectile = base.GetProjectile<ScatterBombProjectile>("ScatterBombProjectile");
        projectile.transform.position = transform.position;

        projectile.explosionRange = explosionRange;
        projectile.scatterCount = scatterCount;


        var direction = target.position - transform.position;
        projectile.Launch(direction.normalized * rangeUntilExplosion + transform.position);
    }

    protected override List<string> GetSpecificUpgradeNames()
    {
        return new List<string>()
        {
            ("GlueExplosionRange"),
            ("GlueScatterCount"),
            ("GlueRangeUntilExplosion")
        };
    }

    protected override void SpecificUpgrade(WeaponUpgrade upgrade)
    {
        upgrade.TryUpgradeSpecific(ref explosionRange, "GlueExplosionRange");
        upgrade.TryUpgradeSpecific(ref scatterCount, "GlueScatterCount");
        upgrade.TryUpgradeSpecific(ref rangeUntilExplosion, "GlueRangeUntilExplosion");
    }
}
