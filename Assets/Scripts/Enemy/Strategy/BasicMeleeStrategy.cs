﻿
using UnityEngine;

public class BasicMeleeStrategy : BehaviorStrategy
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
        else if(!canBasic && distanceToTarget <= basicAttackRange)
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


// TODO for boss
//        // try to make special
//        if(canSpecial && distanceToTarget <= specialAttackRange)
//        {
//            RaycastHit hit;
//Vector3 dir = targetHitPoint - projectileSpawnPoint;
//            if (Physics.Raycast(projectileSpawnPoint, dir, out hit, specialAttackRange, targetMask))
//            {
//                return WhatToDo.SPECIAL_ATTACK;
//            }
//            else
//            {
//                return WhatToDo.MOVE_CLOSER;
//            }
//        }
//        // else try to make basic
//        else if (canBasic && distanceToTarget <= basicAttackRange)
//        {
//            RaycastHit hit;
//Vector3 dir = targetHitPoint - projectileSpawnPoint;
//            if (Physics.Raycast(projectileSpawnPoint, dir, out hit, basicAttackRange, targetMask))
//            {
//                return WhatToDo.BASIC_ATTACK;
//            }
//            else
//            {
//                return WhatToDo.MOVE_CLOSER;
//            }
//        }
//        // move
//        else
//        {
//            return WhatToDo.MOVE_BASIC;
//        }
