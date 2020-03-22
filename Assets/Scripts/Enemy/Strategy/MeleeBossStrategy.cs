using UnityEngine;

public class MeleeBossStrategy : BehaviorStrategyBoss
{
    public override WhatToDoBoss GetNextAction(LayerMask targetMask, Vector3 targetHitPoint, Vector3 projectileSpawnPoint, 
        float distanceToTarget, float basicAttackRange, float specialAttackRange, bool canBasic, bool canSpecial)
    {
        if (canSpecial && distanceToTarget <= specialAttackRange)
        {
            RaycastHit hit;
            Vector3 dir = targetHitPoint - projectileSpawnPoint;
            if (Physics.Raycast(projectileSpawnPoint, dir, out hit, specialAttackRange, targetMask))
            {
                return WhatToDoBoss.SPECIAL_ATTACK;
            }
            else
            {
                return WhatToDoBoss.MOVE_CLOSER;
            }
        }
        // else try to make basic
        else if (canBasic && distanceToTarget <= basicAttackRange)
        {
            RaycastHit hit;
            Vector3 dir = targetHitPoint - projectileSpawnPoint;
            if (Physics.Raycast(projectileSpawnPoint, dir, out hit, basicAttackRange, targetMask))
            {
                return WhatToDoBoss.BASIC_ATTACK;
            }
            else
            {
                return WhatToDoBoss.MOVE_CLOSER;
            }
        }
        // move
        else
        {
            return WhatToDoBoss.MOVE_BASIC;
        }
    }
}
