
using UnityEngine;

public abstract class BehaviorStrategy : MonoBehaviour
{
    public abstract WhatToDo GetNextAction(LayerMask targetMask, Vector3 targetHitPoint, Vector3 projectileSpawnPoint,  float distanceToTarget, float basicAttackRange, bool canBasic);
}

public enum WhatToDo
{
    BASIC_ATTACK,
    MOVE_BASIC,
    MOVE_CLOSER,
    FACE_TARGET,
}
