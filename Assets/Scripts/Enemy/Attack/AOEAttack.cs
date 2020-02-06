using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AOEAttack : CastingAttack
{
    [SerializeField] private ParticleSystem chargingAttackParticle = default;
    [SerializeField] private bool cameraShake = false;

    // ---- INTERN ----
    private Collider aOECollider;
    private int isAttackingUpdate = 0;
    private ParticleSystem particlesCharging;

    void Start()
    {
        aOECollider = GetComponent<Collider>();
        aOECollider.enabled = false;
    }

    protected override void Update()
    {
        base.Update();

        // reset the collider
        if(isAttackingUpdate > 0)
        {
            --isAttackingUpdate;
            aOECollider.enabled = false;
        }
    }

    public override void Perform(Targetable target)
    {
        base.Perform(target);
        particlesCharging = Instantiate(chargingAttackParticle, transform.position, transform.rotation, transform);
    }

    public override void Cast()
    {
        Destroy(particlesCharging.gameObject);
        aOECollider.enabled = true;
        isAttackingUpdate = 2;    // to be sure to detect the collision (if Update is call in the same loop)
        
        if(cameraShake)
        {
            CameraShake.Instance.Shake(0.1f, 0.2f, 0.1f);
        }

        ParticleSystem particles = Instantiate(castingParticle, transform.position, transform.rotation, transform);
        Destroy(particles.gameObject, 3f);
    }

    public override void StopEffects()
    {
        base.StopEffects();
        particlesCharging.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        Targetable targetable = other.transform.GetComponent<Targetable>();
        if (targetable)
        {
            targetable.TakeDamage(damageOnHit, DamageSource.ENEMY);
        }
    }
}
