using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WeaponAttackSpell : WeaponSpell
{
    [SerializeField] protected int damage = 3;
    [SerializeField] protected LayerMask targetMask = default;

    [Header("Setup")]
    [SerializeField] protected Collider trigger = default;

    [SerializeField] private ParticleSystem effectOnHit = default;

    // ---- INTERN ----

    protected override void Update()
    {
        base.Update();
        if(isUsingSpell && wasUsingSpellLastFrame)
        {
            isUsingSpell = false;
            trigger.enabled = false;
            wasUsingSpellLastFrame = false;
        }
        else if(isUsingSpell && !wasUsingSpellLastFrame)
        {
            wasUsingSpellLastFrame = true;
        }
    }

    public override void PerformEffect()
    {
        base.PerformEffect();
        trigger.enabled = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (targetMask == (targetMask | (1 << other.gameObject.layer)))
        {
            Damageable damageable = other.GetComponent<Damageable>();
            if (damageable != null)
            {
                if (other.tag != "Prince" && other.tag == "Enemy")
                {
                    CameraShake.Instance.Shake(0.08f, 0.08f, 0.08f);

                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, transform.up, out hit, 3f, targetMask)) // TODO hitting the player
                    {
                        ParticleSystem ps = Instantiate(effectOnHit, hit.point, Quaternion.identity);
                        Destroy(ps.gameObject, 1f);
                    }
                    else
                    {
                        ParticleSystem ps = Instantiate(effectOnHit, other.transform.position + new Vector3(0f, 1.3f, 0f), Quaternion.identity);
                        Destroy(ps.gameObject, 1f);
                    }
                }

                damageable.TakeDamage(damage, DamageSource.PLAYER);
            }
        }
    }
}
