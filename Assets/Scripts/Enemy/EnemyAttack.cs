using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float Range { get { return range; } }
    public int Damage { get { return damageOnHit; } }

    [SerializeField]
    protected int damageOnHit = 1;
    [SerializeField]
    protected float range = 2f;
}
