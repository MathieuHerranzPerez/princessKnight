using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    [SerializeField]
    protected EnemyStats stats = default;
    [SerializeField]
    protected LayerMask targetMask = default;
    [SerializeField]
    protected EnemyAttack basicAttack = default;
    [SerializeField]
    protected BehaviorStrategy strategy = default;

    [Header("Setup")]
    [SerializeField]
    protected Transform projectileSpawnPoint = default;

    // ---- INERN ----
    protected Animator animator;

    protected Player target = null;
    protected NavMeshAgent navMeshAgent;

    protected bool canAttack = true;

    protected bool isCastingBasic = false;
    protected bool isCastingSpecial = false;

    protected bool isFrozen = false;

    protected EnemyAttack currentAttack;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = stats.defaultSpeed;
        animator = GetComponent<Animator>();
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
            SearchTarge();
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


    // don't forget to call it in animation event
    private void Unfreeze()
    {
        isFrozen = false;
        navMeshAgent.speed = stats.speed;
    }
    protected void Freeze()
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
        Debug.Log("Attack");
        canAttack = false;
        StartCoroutine("CountAttackCouldown");
        // stop the anim during the attaque animation
        // don't forget to Unfreeze with anim event at the end of the animation
        Freeze();
        currentAttack = basicAttack;
        animator.SetTrigger("Attack");
    }

    private void HitTarget()
    {
        target.TakeDamage(currentAttack.Damage);
    }

    private void SearchTarge()
    {
        Collider[] withinAggroCollider;
        withinAggroCollider = Physics.OverlapSphere(transform.position, stats.aggroRange, targetMask);

        if(withinAggroCollider.Length > 0)
        {
            foreach(Collider col in withinAggroCollider)
            {
                if(col.gameObject.tag == "Player")
                {
                    target = col.gameObject.GetComponent<Player>();
                }
            }
        }
    }

    protected virtual void ChasePlayer()
    {
        navMeshAgent.SetDestination(target.transform.position);
        WhatToDo whatToDo = strategy.GetNextAction(targetMask, target.HitTarget.position, projectileSpawnPoint.position, 
            navMeshAgent.remainingDistance, basicAttack.Range, canAttack);

        switch(whatToDo)
        {
            case WhatToDo.BASIC_ATTACK:
                PerformAttack();
                break;

            case WhatToDo.MOVE_BASIC:
                navMeshAgent.stoppingDistance = basicAttack.Range - 0.5f;
                break;

            default:    // move closer
                Debug.Log("moving closer");
                navMeshAgent.stoppingDistance = 0.5f;
                break;
        }
    }


    private IEnumerator CountAttackCouldown()
    {
        float time = stats.attackCouldown;
        while(time > 0f)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        canAttack = true;
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
