using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : AbstractWeapon
{
    [Title("Shotgun")]

    [SerializeField,FoldoutGroup("References")]
    private List<Transform> barrels;

    protected override void Fire(Transform target)
    {
        foreach(var barrel in barrels)
        {
            var bullet = base.GetProjectile<ShotGunBullet>("ShotGunProjectile");
            bullet.Shoot(barrel.transform.position - transform.position);
        }
    }

    protected override List<string> GetSpecificUpgradeNames()
    {
        return new List<string>();
    }

    protected override void SpecificUpgrade(WeaponUpgrade upgrade)
    {
        
    }

    private void OnDrawGizmos()
    {
        foreach (var barrel in barrels)
        {
            Gizmos.DrawLine(transform.position, barrel.transform.position);
        }
    }
}
