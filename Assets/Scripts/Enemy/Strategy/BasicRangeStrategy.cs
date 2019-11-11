
using UnityEngine;

public class BasicRangeStrategy : BehaviorStrategy
{
    public override WhatToDo GetNextAction(LayerMask targetMask, Vector3 targetHitPoint, Vector3 projectileSpawnPoint, float distanceToTarget, float basicAttackRange, bool canBasic)
    {
        if (canBasic && distanceToTarget <= basicAttackRange)
        {
            RaycastHit hit;
            Vector3 dir = targetHitPoint - projectileSpawnPoint;
            if (Physics.Raycast(projectileSpawnPoint, dir, out hit, basicAttackRange, targetMask))
            {
                return WhatToDo.BASIC_ATTACK;
            }
            else
            {
                return WhatToDo.MOVE_CLOSER;
            }
        }
        // face the target
        else if (!canBasic && distanceToTarget <= basicAttackRange)
        {
            return WhatToDo.FACE_TARGET;
        }
        // move
        else
        {
            return WhatToDo.MOVE_BASIC;
        }
    }
}
