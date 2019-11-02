using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour, Damageable
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

    // ---- INERN ----
    protected Animator animator;

    protected Targetable target = null;
    protected NavMeshAgent navMeshAgent;

    protected bool isCastingBasic = false;
    protected bool isCastingSpecial = false;

    protected bool isFrozen = false;
    protected bool isRotationFrozen = false;
    protected bool isAttacking = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = stats.defaultSpeed;
        animator = GetComponent<Animator>();
        attack.Source = this;
    }

    void Update()
    {
        if(target != null)
        {
            ChasePlayer();
        }
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            SearchTarget();
        }
    }


    public void TakeDamage(int amount)
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

    protected void Die()
    {
        // todo effect

        Destroy(transform.gameObject, 0f);
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
}
