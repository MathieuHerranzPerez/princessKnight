using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FireHeadAttack : EnemyAttack
{
    // ---- INTERN ----
    private Collider attackTrigger;

    protected virtual void Start()
    {
        attackTrigger = GetComponent<Collider>();
    }

    public override void Perform(Targetable target)
    {
        attackTrigger.enabled = true;
    }

    public override void End()
    {
        attackTrigger.enabled = false;
    }

    public override void Cast()
    {
        attackTrigger.enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        Targetable targetable = other.transform.GetComponent<Targetable>();
        if (targetable)
        {
            targetable.TakeDamage(damageOnHit, DamageSource.ENEMY);
        }
    }
}
