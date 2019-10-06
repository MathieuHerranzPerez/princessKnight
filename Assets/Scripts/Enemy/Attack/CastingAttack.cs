using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CastingAttack : EnemyAttack
{
    [SerializeField]
    protected ParticleSystem castingParticle = default;
}
