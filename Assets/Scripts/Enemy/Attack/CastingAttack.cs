using UnityEngine;

public abstract class CastingAttack : EnemyAttack
{
    [SerializeField]
    protected ParticleSystem castingParticle = default;
}
