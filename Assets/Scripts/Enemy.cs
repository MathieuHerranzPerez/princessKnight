using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    protected Stats stats = default;

    public void TakeDamage(int amount)
    {
        stats.HP -= amount;

        if(stats.HP < 0)
        {
            Die();
        }
    }

    public void Die()
    {
        // todo effect

        Destroy(transform.gameObject, 1.5f);
    }
}
