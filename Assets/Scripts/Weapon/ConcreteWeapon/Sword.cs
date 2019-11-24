using UnityEngine;

public class Sword : WeaponObjectAttack
{
    protected override void hitDamageable(Collider other, Damageable otherDamageable)
    {
        if(other.tag != "Player")
        {
            otherDamageable.TakeDamage(weapon.stats.damage, DamageSource.PLAYER);
        }
    }
}
