using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy, Observable
{
    public int Difficulty { get { return difficulty; } }

    [SerializeField]
    protected new BossStats stats = default;
    [SerializeField]
    protected BehaviorStrategyBoss strategy2 = default;
    [SerializeField]
    protected EnemyAttack specialAttack = default;


    // the difficulty is set between 1 and 10. If this has to change, check all the references
    [Range(1, 10)]
    [SerializeField]
    private int difficulty = 5;

    // ---- INTERN ----
    protected EnemyAttack currentAttack;
    private List<Observer> listObserver = new List<Observer>();

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

    protected override void Die()
    {
        base.Die();

        foreach(Observer obs in listObserver)
        {
            obs.Notify();
        }
    }

    public void Register(Observer obsever)
    {
        listObserver.Add(obsever);
    }
}
