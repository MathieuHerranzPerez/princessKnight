using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(HerdUnit))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Prince : Targetable, INavMeshUnit
{
    [SerializeField]
    private PrinceStats stats;

    // ---- INTERN ----
    private NavMeshAgent navMeshAgent;
    private PrinceStatus status = PrinceStatus.WaitingHero;
    private HerdUnit herdUnit;

    protected bool isOnNavMesh = true;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        herdUnit = GetComponent<HerdUnit>();
    }

    private void Update()
    {
        if (isOnNavMesh)
        {
            if (status == PrinceStatus.JoiningHerd && !herdUnit.IsMemberOfHerd)
            {
                navMeshAgent.SetDestination(HerdManager.Instance.Herd.target.position);
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, HerdManager.Instance.Herd.target.position, herdUnit.Speed * Time.deltaTime);
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

    public void LeaveNavMesh()
    {
        herdUnit.LeaveNavMesh();
        navMeshAgent.enabled = false;
        isOnNavMesh = false;
    }

    public void EnterOnNavMesh()
    {
        navMeshAgent.enabled = true;
        herdUnit.EnterOnNavMesh();
        isOnNavMesh = true;
    }

    private void Die()
    {
        // TODO anim
        ScoreManager.Instance.NotifyPrinceDeath();
        herdUnit.LeaveHerd();
        Destroy(transform.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2f);
    }
}

enum PrinceStatus
{
    WaitingHero,
    JoiningHerd,
    InHerd,
}
