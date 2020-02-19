using UnityEngine;

public class HerdLeader : MonoBehaviour
{
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
        transform.position = herd.agentTargetPosition;
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
