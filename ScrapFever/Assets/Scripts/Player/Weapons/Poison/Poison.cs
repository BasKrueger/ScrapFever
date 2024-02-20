using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class Poison : AbstractWeapon
{
    [SerializeField, TabGroup("Specifics")]
    private float puddleSize;
    [SerializeField,TabGroup("Specifics"), SuffixLabel("damages per second", true)]
    private float poisonDamageRate;
    [SerializeField, TabGroup("Specifics"), SuffixLabel("seconds", true)]
    private float puddleDuration;
    
    protected override void Fire(Transform target)
    {
        var projectile = base.GetProjectile<PoisonProjectile>("PoisonProjectile");
        projectile.puddleDamageRate = poisonDamageRate;
        projectile.puddleScale = puddleSize;
        projectile.puddleDuration = this.puddleDuration;

        projectile.Launch(target.position);
    }

    protected override List<string> GetSpecificUpgradeNames()
    {
        return new List<string>()
        {
            ("GluePuddleSize"),
            ("GluePoisonDamageRate"),
            ("GluePuddleDuration")
        };
    }

    protected override void SpecificUpgrade(WeaponUpgrade upgrade)
    {
        upgrade.TryUpgradeSpecific(ref puddleSize, "GluePuddleSize");
        upgrade.TryUpgradeSpecific(ref poisonDamageRate, "GluePoisonDamageRate");
        upgrade.TryUpgradeSpecific(ref puddleDuration, "GluePuddleDuration");
    }
}
