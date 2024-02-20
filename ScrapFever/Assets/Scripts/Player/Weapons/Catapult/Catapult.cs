using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class Catapult : AbstractWeapon
{
    [SerializeField, TabGroup("Specifics")]
    private int bounces = 3;
    [SerializeField, TabGroup("Specifics"), SuffixLabel("per bounce", true)]
    private float bounceDistance = 3;

    protected override void Fire(Transform target)
    {
        var projectile = base.GetProjectile<CatapultProjectile>("CatapultProjectile");
        projectile.bounces = this.bounces + 1;
        projectile.bounceDistance = this.bounceDistance;

        projectile.Launch(target.transform);
    }

    protected override List<string> GetSpecificUpgradeNames()
    {
        return new List<string>()
        {
            ("GlueBounces"),
            ("GlueBounceDistance")
        };
    }

    protected override void SpecificUpgrade(WeaponUpgrade upgrade)
    {
        upgrade.TryUpgradeSpecific(ref bounces, "GlueBounces");
        upgrade.TryUpgradeSpecific(ref bounceDistance, "GlueBounceDistance");
    }
}
