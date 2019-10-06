using UnityEngine;

public class Boss : Enemy
{

    [SerializeField]
    protected new BossStats stats = default;
    [SerializeField]
    protected BehaviorStrategyBoss strategy2 = default;
    [SerializeField]
    protected EnemyAttack specialAttack = default;

    // ---- INTERN ----
    protected EnemyAttack currentAttack;

    protected void PerformSpecial()
    {
        Debug.Log("Special");
        specialAttack.Perform(target);
    }

    protected override void ChasePlayer()
    {
        navMeshAgent.SetDestination(target.transform.position);
        WhatToDoBoss whatToDo = this.strategy2.GetNextAction(targetMask, target.HitTargetPoint, projectileSpawnPoint.position,
            navMeshAgent.remainingDistance, attack.Range, specialAttack.Range, attack.Couldown <= 0f, specialAttack.Couldown <= 0f);

        switch (whatToDo)
        {
            case WhatToDoBoss.BASIC_ATTACK:
                PerformAttack();
                break;

            case WhatToDoBoss.SPECIAL_ATTACK:
                PerformSpecial();
                break;

            case WhatToDoBoss.MOVE_BASIC:
                navMeshAgent.stoppingDistance = attack.Range - 0.5f;
                break;

            case WhatToDoBoss.MOVE_SPECIAL:
                navMeshAgent.stoppingDistance = specialAttack.Range - 0.5f;
                break;

            default:    // move closer
                Debug.Log("moving closer");
                navMeshAgent.stoppingDistance = 0.5f;
                break;
        }
    }
}
