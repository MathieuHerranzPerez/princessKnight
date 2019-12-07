using UnityEngine;

public class RandomMoveStrategy : BehaviorStrategy
{
    public override WhatToDo GetNextAction(LayerMask targetMask, Vector3 targetHitPoint, Vector3 projectileSpawnPoint, float distanceToTarget, float basicAttackRange, bool canBasic)
    {
        Vector3 dir = targetHitPoint - projectileSpawnPoint;
        float distSquare = dir.x * dir.x + dir.z * dir.z;
        if(distSquare > 170f)
        {
            return WhatToDo.MOVE_CLOSER;
        }
        return WhatToDo.MOVE_BASIC;
    }
}
