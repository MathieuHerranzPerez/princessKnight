using UnityEngine;

public abstract class ColliderAttack : ColliderAttackDef
{
    [Header("Setup")]
    [SerializeField]
    private ParticleSystem effectOnHit = default;
    [SerializeField]
    private LayerMask enemyLayerMask = default;

    protected abstract void HitDamageable(Collider other, Damageable otherDamageable);

    protected override void ActOnTriggerEnter(Collider other)
    {
        Damageable damageable = other.GetComponent<Damageable>();
        if (damageable != null)
        {
            if (other.tag != "Prince" && other.tag == "Enemy")
            {
                CameraShake.Instance.Shake(0.08f, 0.08f, 0.08f);

                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.up, out hit, 3f, enemyLayerMask)) // TODO hitting the player
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


            HitDamageable(other, damageable);
        }
    }
}
