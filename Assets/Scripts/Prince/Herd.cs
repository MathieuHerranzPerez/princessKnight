using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Herd : MonoBehaviour
{
    

    public HerdLeader Leader { get { return virtualLeader; } }
    public float Speed { get { return herdSpeed; } }
    public bool IsEmpty { get { return unitList.Count == 0; } }
    public float Radius { get { return radius; } }
    public float RadiusSquare { get { return radiusSquare; } }
    public float MaxSpeed { get { return maxUnitSpeed; } }
    public Transform target;

    public int size = 0;

    [SerializeField]
    private float herdSpeed = 5f;
    [SerializeField]
    private float maxUnitSpeed = 10f;
    [SerializeField]
    private HerdLeader virtualLeader = default;

    [Header("Setup")]
    [SerializeField]
    private Transform unitsContainer = default;
    [SerializeField]
    private AnimationCurve radiusByUnitCurve = default;

    // ---- INTERN ----
    private List<HerdUnit> unitList = new List<HerdUnit>();
    private HerdState state = HerdState.Formed;
    private float radius = 1f;
    private float radiusSquare = 1f;
    private SphereCollider trigger;

    void Start()
    {
        trigger = GetComponent<SphereCollider>();
        trigger.radius = radius;
        virtualLeader.JoinHerd(this);
    }

    void Update()
    {
        // virtualLeader.SetTarget(target.transform.position);
        trigger.center = virtualLeader.transform.position;

        if (state == HerdState.Formed) 
        {
            foreach (HerdUnit hu in unitList)
            {
                Vector3 gravityCenter = GetGravityCenter();
                Vector3 diffAgent = hu.transform.position - gravityCenter;
                float distSquare = diffAgent.x * diffAgent.x + diffAgent.y + diffAgent.y;
                if (distSquare >= radiusSquare && !hu.IsOnHisWay)
                {
                    state = HerdState.Broken;
                    break;
                }
            }
        }
        else if(state == HerdState.Forming)
        {
            Vector3 gravityCenter = GetGravityCenter();
            Vector3 diff = virtualLeader.transform.position - gravityCenter;
            float distanceBetweenGravityAndMiddleSquare = diff.x * diff.x + diff.z * diff.z;
            if (distanceBetweenGravityAndMiddleSquare > 1.5f)
            {
                foreach(HerdUnit hu in unitList)
                {
                    hu.MoveToFormGroup(diff);
                }
            }
            else
            {
                state = HerdState.Formed;
                foreach (HerdUnit hu in unitList)
                {
                    Vector3 diffAgent = hu.transform.position - gravityCenter;
                    float distSquare = diffAgent.x * diffAgent.x + diffAgent.y + diffAgent.y;

                    if (distSquare >= radiusSquare && !hu.IsOnHisWay)
                    {
                        MoveAllAgentToCenter(gravityCenter);
                        state = HerdState.Forming;
                        break;
                    }
                }
            }
        }
        else if (state == HerdState.Broken)
        {
            state = HerdState.Forming;
        }
    }

    public void AddUnit(HerdUnit unit)
    {
        ++size;
        unit.JoinHerd(this);
        unitList.Add(unit);
        unit.transform.parent = unitsContainer;
        state = HerdState.Broken;
        radius = radiusByUnitCurve.Evaluate(unitList.Count);
        radiusSquare = radius * radius;
        trigger.radius = radius;
    }

    public void RemoveUnit(HerdUnit unit)
    {
        unitList.Remove(unit);
        state = HerdState.Broken;
    }

    public Vector3 GetGravityCenter()
    {
        Vector3 center = Vector3.zero;
        if (unitList.Count > 0)
        {
            foreach (HerdUnit hu in unitList)
            {
                center += hu.transform.position;
            }
            center /= (float)unitList.Count;
        }
        return center;
    }

    private void MoveAllAgentToCenter(Vector3 gravityCenter)
    {
        foreach (HerdUnit hu in unitList)
        {
            hu.MoveTo(gravityCenter);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        HerdUnit hu = other.GetComponent<HerdUnit>();
        if (hu && !hu.IsMemberOfHerd)
        {
            AddUnit(hu);
        }
    }

    /**
     *  Show the aggro range in gizmos and the gravity center
     */
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(virtualLeader.transform.position, radius);
        Gizmos.color = Color.black;
        Vector3 g = GetGravityCenter();
        Gizmos.DrawLine(g, g + Vector3.up * 3);
    }

}


enum HerdState
{
    Broken,
    Forming,
    Formed
};
