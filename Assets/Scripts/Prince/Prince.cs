﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(HerdUnit))]
[RequireComponent(typeof(Collider))]
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
            gameObject.transform.parent = null;    // put it in the scene root to not be removed if mapFragement destroy
        }
    }

    public void NotifySaved()
    {
        Collider collider = GetComponent<Collider>();
        collider.enabled = false;

        // todo anim

        Destroy(gameObject, 1f);
    }

    private void Die()
    {
        // TODO anim
        ScoreManager.Instance.NotifyPrinceDeath();
        herdUnit.LeaveHerd();
        Destroy(transform.gameObject);
    }
}

enum PrinceStatus
{
    WaitingHero,
    JoiningHerd,
    InHerd,
}
