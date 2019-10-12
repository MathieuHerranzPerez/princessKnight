using UnityEngine;

public abstract class WeaponObjectAttack : WeaponObject
{
    protected abstract void hitDamageable(Collider other, Damageable otherDamageable);

    protected void OnTriggerEnter(Collider other)
    {
        Damageable damageable = other.GetComponent<Damageable>();
        if (damageable != null)
        {
            hitDamageable(other, damageable);
        }
    }
}
