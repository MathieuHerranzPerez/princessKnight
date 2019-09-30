using System.Collections;
using UnityEngine;

public class Boss : Enemy
{

    [SerializeField]
    protected new BossStats stats = default;
    [SerializeField]
    protected new BehaviorStrategyBoss strategy = default;
    [SerializeField]
    protected EnemyAttack specialAttack = default;

    // ---- INTERN ----
    protected bool canSpecial = true;

    protected void PerformSpecial()
    {
        Debug.Log("Special");
        canSpecial = false;
        StartCoroutine("CountSpecialCouldown");
        Freeze();
        currentAttack = specialAttack;
        animator.SetTrigger("special");
    }

    protected override void ChasePlayer()
    {
        navMeshAgent.SetDestination(target.transform.position);
        WhatToDoBoss whatToDo = strategy.GetNextAction(targetMask, target.HitTarget.position, projectileSpawnPoint.position,
            navMeshAgent.remainingDistance, basicAttack.Range, specialAttack.Range, canAttack, canSpecial);

        switch (whatToDo)
        {
            case WhatToDoBoss.BASIC_ATTACK:
                PerformAttack();
                break;

            case WhatToDoBoss.SPECIAL_ATTACK:
                PerformSpecial();
                break;

            case WhatToDoBoss.MOVE_BASIC:
                navMeshAgent.stoppingDistance = basicAttack.Range - 0.5f;
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

    private IEnumerator CountSpecialCouldown()
    {
        float time = stats.specialCouldown;
        while (time > 0f)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        canSpecial = true;
    }
}
