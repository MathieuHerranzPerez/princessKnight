
using UnityEngine;

public abstract class BehaviorStrategy : MonoBehaviour
{
    public abstract WhatToDo GetNextAction(LayerMask targetMask, Vector3 targetHitPoint, Vector3 projectileSpawnPoint,  float distanceToTarget, float basicAttackRange, float specialAttackRange, bool canBasic, bool canSpecial);
}

public enum WhatToDo
{
    BASIC_ATTACK,
    SPECIAL_ATTACK,
    MOVE_BASIC,
    MOVE_SPECIAL,
    MOVE_CLOSER,
}
