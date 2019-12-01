using UnityEngine;

public class SwordCollider : ColliderAttack
{
    protected override void HitDamageable(Collider other, Damageable otherDamageable)
    {
        if (other.tag != "Player")
        {
            otherDamageable.TakeDamage(weapon.stats.damage, DamageSource.PLAYER);
        }
    }
}
