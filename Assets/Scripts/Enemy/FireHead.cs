using UnityEngine;

public class FireHead : Enemy
{
    [SerializeField]
    private ParticleSystem fireParticles = default;

    // ---- INTERN ----
    private Vector3 nextPos;
    private float timeAfterNewDest = 0f;
    private Vector3 firstPos;

    protected override void Start()
    {
        base.Start();
        nextPos = transform.position;
        firstPos = transform.position;
    }

    protected override void Die()
    {
        base.Die();
        Destroy(transform.gameObject, 1f);
        fireParticles.Stop();
    }

    protected override void ChasePlayer()
    {
        timeAfterNewDest += Time.deltaTime;

        Vector3 dirToNextPos = transform.position - nextPos;
        float squareDistToNextPos = dirToNextPos.x * dirToNextPos.x + dirToNextPos.z * dirToNextPos.z;
        if (squareDistToNextPos < 1.5f || timeAfterNewDest >= 3f)
        {
            WhatToDo whatToDo = strategy.GetNextAction(targetMask, target.HitTargetPoint, projectileSpawnPoint.position,
                navMeshAgent.remainingDistance, attack.Range, attack.Couldown <= 0f);
            switch (whatToDo)
            {
                case WhatToDo.MOVE_BASIC:
                    Vector3 dirToFirstPos = transform.position - firstPos;
                    float squareDistToFirstPos = dirToFirstPos.x * dirToFirstPos.x + dirToFirstPos.z * dirToFirstPos.z;
                    if (squareDistToFirstPos > 170f)
                    {
                        Vector3 targetPosition = target.transform.position;
                        float rand = Random.Range(0.5f, 1f) * 10f;
                        bool isRandPositive = Random.value > 0.5f;
                        if (dirToFirstPos.x * dirToFirstPos.x > dirToFirstPos.z * dirToFirstPos.z)
                        {
                            targetPosition.z = rand * (isRandPositive ? 1f : -1f) + transform.position.z;
                        }
                        else
                        {
                            targetPosition.x = rand * (isRandPositive ? 1f : -1f) + transform.position.x;
                        }
                        nextPos = targetPosition;
                    }
                    else
                    {
                        // get random destination around
                        float randomX = Random.Range(0.5f, 1f) * 10f;
                        float randomZ = Random.Range(0.5f, 1f) * 10f;
                        bool isXPositive = Random.value > 0.5f;
                        bool isZPositive = Random.value > 0.5f;
                        Vector3 randomPos = transform.position;
                        nextPos = randomPos + new Vector3(randomX * (isXPositive ? 1f : -1f), 0f, randomZ * (isZPositive ? 1f : -1f));
                    }
                    
                    navMeshAgent.SetDestination(nextPos);
                    break;

                default:    // move closer
                    Vector3 dirToTarget = target.HitTargetPoint - transform.position;
                    Vector3 targetPos = target.transform.position;
                    float random = Random.Range(0.5f, 1f) * 10f;
                    bool isRandomPositive = Random.value > 0.5f;
                    if (dirToTarget.x * dirToTarget.x > dirToTarget.z * dirToTarget.z)
                    {
                        targetPos.z = random * (isRandomPositive ? 1f : -1f) + transform.position.z;
                    }
                    else
                    {
                        targetPos.x = random * (isRandomPositive ? 1f : -1f) + transform.position.x;
                    }
                    nextPos = targetPos;
                    navMeshAgent.SetDestination(nextPos);
                    break;
            }

            timeAfterNewDest = 0f;
        }
    }
}
