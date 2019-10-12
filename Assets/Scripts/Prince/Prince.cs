using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(HerdUnit))]
public class Prince : Targetable
{
    [SerializeField]
    private PrinceStats stats;

    // ---- INTERN ----
    private NavMeshAgent navMeshAgent;
    private PrinceStatus status = PrinceStatus.WaitingHero;
    private HerdUnit herdUnit;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        herdUnit = GetComponent<HerdUnit>();
    }

    private void Update()
    {
        if(status == PrinceStatus.JoiningHerd && !herdUnit.IsMemberOfHerd)
        {
            navMeshAgent.SetDestination(HerdManager.Instance.Herd.target.position);
        }
    }

    public override void TakeDamage(int amount)
    {
        if (status != PrinceStatus.WaitingHero)
        {
            stats.HP -= amount;
            // TODO anim
            if (stats.HP <= 0)
            {
                Die();
            }
        }
        else
        {
            // todo anim
            status = PrinceStatus.JoiningHerd;
        }
    }

    private void Die()
    {
        // TODO anim
        ScoreManager.Instance.NotifyPrinceDeath();
        Destroy(transform.gameObject);
    }
}

enum PrinceStatus
{
    WaitingHero,
    JoiningHerd,
    InHerd,
}
