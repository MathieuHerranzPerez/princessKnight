using UnityEngine;

public abstract class BehaviorStrategyBoss : MonoBehaviour
{
    public abstract WhatToDoBoss GetNextAction(LayerMask targetMask, Vector3 targetHitPoint, Vector3 projectileSpawnPoint, float distanceToTarget, float basicAttackRange, float specialAttackRange, bool canBasic, bool canSpecial);
}


public enum WhatToDoBoss
{
    BASIC_ATTACK,
    SPECIAL_ATTACK,
    MOVE_BASIC,
    MOVE_SPECIAL,
    MOVE_CLOSER,
}
