using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerdLeader : MonoBehaviour
{
    [SerializeField]
    private float maxHerdDrift = 4f;

    // ---- INTERN ----
    private Vector3 targetPos;
    private float maxHerdDriftSquare;
    protected Herd herd;

    protected void Start()
    {
        // maxHerdDriftSquare = maxHerdDrift * maxHerdDrift;
    }

    protected void Update()
    {
        // ChangeSpeed();
    }

    void FixedUpdate()
    {
        transform.position = herd.target.position;
    }

    //private void ChangeSpeed()
    //{
    //    if (!herd.IsEmpty)
    //    {
    //        Vector3 diff = transform.position - herd.GetGravityCenter();
    //        float distanceSquare = diff.x * diff.x + diff.z * diff.z;
    //        float speedModifier = (maxHerdDriftSquare - distanceSquare) / maxHerdDriftSquare;
    //        navMeshAgent.speed = herd.Speed * speedModifier;
    //    }
    //}

    //public void SetTarget(Vector3 targetPos)
    //{
    //    navMeshAgent.SetDestination(targetPos);
    //}
    public void JoinHerd(Herd herd)
    {
        this.herd = herd;
    }
}
