using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class Spark : AbstractProjectile
{
    [Title("Spark")]

    [HideInInspector]
    public IntStat bounces { private get; set; }

    [SerializeField]
    private int bounceRange;

    private List<GameObject> hitEnemies = new List<GameObject>();

    public override void Destroy()
    {
        Pool.Return<Spark>(this.gameObject);
    }

    protected override void Hit(AbstractEnemy enemy)
    {
        bounces -= 1;
        Damage(enemy);

        if(bounces > 1)
        {
            JumpToNextEnemy();
            return;
        }

        Destroy();
    }

    private void Damage(AbstractEnemy enemy)
    {
        enemy.TakeDamage(base.damage, base.source);
        hitEnemies.Add(enemy.gameObject);
    }

    private void JumpToNextEnemy()
    {
        var targetPosition = GetNextTargetPosition();
        if (targetPosition == new Vector3())
        {
            Destroy();
        }

        base.Shoot(targetPosition - transform.position);
    }

    protected override void InternalOnTakenFromPool()
    {
        bounces?.Reset();
        hitEnemies = new List<GameObject>();
    }

    private Vector3 GetNextTargetPosition()
    {
        GameObject closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (var enemy in Pool.GetOutOfPools<AbstractEnemy>())
        {
            if(hitEnemies.Contains(enemy)) { continue; }

            var distance = Vector3.Distance(enemy.transform.position, transform.position);
            if (distance < bounceRange && distance < closestDistance)
            {
                closest = enemy;
                closestDistance = distance;
            }
        }

        if (closest == null) return new Vector3();

        return closest.transform.position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, bounceRange);
    }
}
