public class ShotGunBullet : AbstractProjectile
{
    public override void Destroy()
    {
        Pool.Return<ShotGunBullet>(this.gameObject);
    }

    protected override void Hit(AbstractEnemy enemy)
    {
        enemy.TakeDamage(base.damage, base.source);
        Destroy();
    }
}
