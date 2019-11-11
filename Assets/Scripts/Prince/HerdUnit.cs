using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class HerdUnit : MonoBehaviour
{
    public bool IsMemberOfHerd { get { return hasReachTheHerd; } }
    public bool IsOnHisWay { get { return isMovingFaster; } }

    // ---- INTERN ----
    protected Herd herd;
    protected NavMeshAgent navMeshAgent;
    protected bool hasReachTheHerd = false;
    protected Vector3 offset = Vector3.zero;
    protected bool isFormingGroup = false;
    protected bool isMovingFaster = false;
    protected bool isLeavingHerd = false;

    protected virtual void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Update()
    {
        if(!isFormingGroup && hasReachTheHerd && !isLeavingHerd)
        {
            navMeshAgent.speed = herd.Speed;
            MoveToNextPosition();
        }
        else if(isFormingGroup && hasReachTheHerd && !isLeavingHerd)
        {
            offset = transform.position - herd.Leader.transform.position;
        }

        isFormingGroup = false;
    }

    public void MoveToFormGroup(Vector3 direction)
    {
        isFormingGroup = true;
        Vector3 diffCenterToPos = (transform.position + direction) - herd.Leader.transform.position;
        float distanceToGroupCenterSquare = diffCenterToPos.x * diffCenterToPos.x + diffCenterToPos.z * diffCenterToPos.z;
        // if the desired pos is out of the group radius
        if (distanceToGroupCenterSquare > herd.RadiusSquare)
        {
            direction += -1f * diffCenterToPos;
        }

        navMeshAgent.SetDestination(transform.position + direction);
    }

    public void MoveTo(Vector3 point)
    {
        isFormingGroup = true;
        navMeshAgent.SetDestination(point);
    }

    public void JoinHerd(Herd herd)
    {
        this.herd = herd;
        hasReachTheHerd = true;
        offset = transform.position - herd.Leader.transform.position;

        //navMeshAgent.angularSpeed = 120f;
        navMeshAgent.acceleration = 3f;
    }

    public void LeaveHerd()
    {
        if (hasReachTheHerd)
        {
            herd.RemoveUnit(this);
        }
    }

    public void LeaveHerdToGoingPos(Vector3 targetPos)
    {
        isLeavingHerd = true;
        navMeshAgent.acceleration = 300f;
        navMeshAgent.speed = navMeshAgent.speed * 2f;
        navMeshAgent.SetDestination(targetPos);
    }

    private void MoveToNextPosition()
    {
        Vector3 targetPos = herd.Leader.transform.position + offset;
        Vector3 dir = transform.position - targetPos;
        float distanceSquare = dir.x * dir.x + dir.z * dir.z;

        navMeshAgent.SetDestination(targetPos);
        if (distanceSquare >= 3f)
        {
            isMovingFaster = true;
            navMeshAgent.speed = herd.MaxSpeed;
        } 
        else
        {
            isMovingFaster = false;
            navMeshAgent.speed = herd.Speed;
        }
    }
}
