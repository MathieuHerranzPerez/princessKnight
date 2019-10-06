using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObjectAttack : WeaponObject
{

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            Enemy enemyHit = other.GetComponent<Enemy>();
            enemyHit.TakeDamage(weapon.stats.damage);
        }
    }
}
