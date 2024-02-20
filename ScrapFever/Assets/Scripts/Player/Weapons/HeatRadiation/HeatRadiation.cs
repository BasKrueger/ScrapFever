using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class HeatRadiation : AbstractWeapon
{
    [TabGroup("Specifics"), SerializeField, OnValueChanged("UpdateScale")]
    private float range = 1;

    [FoldoutGroup("References"), SerializeField]
    private DamageZone zone;
    [FoldoutGroup("References"), SerializeField]
    private Blinker blinker;
    [FoldoutGroup("References"), SerializeField]
    private List<Transform> scaleWithRadius;

    protected override void InternalAwake()
    {
        zone.damage = base.damage;
        zone.cooldown = 1 / base.fireRate;

        zone.overrideSource = this;
        UpdateScale();
    }

    protected override void Fire(Transform target)
    {
    }

    private void UpdateScale()
    {
        foreach(Transform t in scaleWithRadius)
        {
            t.localScale = new Vector3(range * 2, range * 2, range * 2);
        }

        zone.GetComponentInChildren<CapsuleCollider>().radius = range;
    }

    protected override List<string> GetSpecificUpgradeNames()
    {
        return new List<string>()
        {
            ("GlueRange"),
        };
    }

    protected override void SpecificUpgrade(WeaponUpgrade upgrade)
    {
        upgrade.TryUpgradeSpecific(ref range, "GlueRange");

        zone.damage = base.damage;
        zone.cooldown = 1 / base.fireRate;

        UpdateScale();
    }
}
