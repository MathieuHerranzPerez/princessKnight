using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour, Damageable, INavMeshUnit
{
    public Animator Anim { get { return animator; } }
    public Transform ProjecileSpawnPoint { get { return projectileSpawnPoint; } }


    [SerializeField]
    protected EnemyStats stats = default;
    [SerializeField]
    protected LayerMask targetMask = default;
    [SerializeField]
    protected string targetTag = "Player";
    [SerializeField]
    protected EnemyAttack attack = default;
    [SerializeField]
    protected BehaviorStrategy strategy = default;

    [Header("Setup")]
    [SerializeField]
    protected Transform projectileSpawnPoint = default;
    [SerializeField]
    private MeshRenderer enemyMesh = default;

    // ---- INERN ----
    protected Animator animator;
    protected Collider colliderE;

    protected Targetable target = null;
    protected NavMeshAgent navMeshAgent;

    protected bool isCastingBasic = false;
    protected bool isCastingSpecial = false;

    protected bool isFrozen = false;
    protected bool isRotationFrozen = false;
    protected bool isAttacking = false;

    protected bool isDying = false;
    protected Color alphaColor;

    protected bool isOnNavMesh = true;
    protected Rigidbody rb;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = stats.defaultSpeed;
        animator = GetComponent<Animator>();
        colliderE = GetComponent<Collider>();
        attack.Source = this;

        alphaColor = enemyMesh.material.color;
        alphaColor.a = 0;

        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isOnNavMesh)
        {
            if (target != null && !isDying)
            {
                ChasePlayer();
            }
            else if (isDying)
            {
                enemyMesh.material.color = Color.Lerp(enemyMesh.material.color, alphaColor, 1f * Time.deltaTime);
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, stats.speed * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            SearchTarget();
        }
    }


    public void TakeDamage(int amount, DamageSource source)
    {
        stats.HP -= amount;

        if(stats.HP <= 0)
        {
            Die();
        }
    }


    public void Unfreeze()
    {
        isFrozen = false;
        navMeshAgent.speed = stats.speed;
        navMeshAgent.angularSpeed = 300f;
    }

    public void FreezeRotaion()
    {
        isRotationFrozen = true;
        navMeshAgent.angularSpeed = 0f;
    }

    public void FreezeMovement()
    {
        isFrozen = true;
        navMeshAgent.speed = 0f;
    }

    public void LeaveNavMesh()
    {
        isOnNavMesh = false;
        navMeshAgent.enabled = false;
    }

    public void EnterOnNavMesh()
    {
        isOnNavMesh = true;
        navMeshAgent.enabled = true;
    }

    protected void Die()
    {
        isDying = true;
        colliderE.enabled = false;
        // todo effect

        animator.SetTrigger("Dying");

        StatisticsManager.Instance.NotifyEnemyDeath(this);
        Destroy(transform.gameObject, 2f);
    }

    protected void PerformAttack()
    {
        attack.Perform(target);
        isAttacking = true;
    }

    // don't forget to call it in animation event
    protected void FinishAttack()
    {
        attack.End();
        isAttacking = false;
    }

    private void Cast()
    {
        attack.Cast();
    }

    private void SearchTarget()
    {
        Collider[] withinAggroCollider;
        withinAggroCollider = Physics.OverlapSphere(transform.position, stats.aggroRange, targetMask);

        if(withinAggroCollider.Length > 0)
        {
            foreach(Collider col in withinAggroCollider)
            {
                if(col.gameObject.tag == targetTag)
                {
                    target = col.gameObject.GetComponent<Targetable>();
                    attack.Target = target;
                    gameObject.transform.parent = null;    // put it in the scene root to not be removed if mapFragement destroy
                }
            }
        }
    }

    protected virtual void ChasePlayer()
    {
        navMeshAgent.SetDestination(target.transform.position);
        //Debug.Log((transform.position - target.transform.position).magnitude);
        WhatToDo whatToDo = strategy.GetNextAction(targetMask, target.HitTargetPoint, projectileSpawnPoint.position, 
            navMeshAgent.remainingDistance, attack.Range, attack.Couldown <= 0f);

        bool hasToLookAtTarget = false;

        switch (whatToDo)
        {
            case WhatToDo.BASIC_ATTACK:
                PerformAttack();
                break;

            case WhatToDo.MOVE_BASIC:
                navMeshAgent.stoppingDistance = attack.Range - 0.5f;
                break;

            case WhatToDo.FACE_TARGET:
                hasToLookAtTarget = true;
                break;

            default:    // move closer
                navMeshAgent.stoppingDistance = 0.5f;
                break;
        }

        // if the enemy is performing a non frozen attack or if he doesn't need to move to attack the player
        if (hasToLookAtTarget || (isFrozen && !isRotationFrozen && isAttacking))
        {
            transform.LookAt(target.transform);
        }
    }

    /**
     *  Show the aggro range in gizmos
     */
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stats.aggroRange);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2f);
    }
}
