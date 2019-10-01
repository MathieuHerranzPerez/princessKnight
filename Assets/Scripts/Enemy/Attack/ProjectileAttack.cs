using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ProjectileAttack : CastingAttack
{
    [SerializeField]
    protected GameObject projectilePrefab = default;
    [SerializeField]
    protected float projectileTimeToLive = 4f;

    // ---- INTERN ----
    private LineRenderer lineRenderer;


    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public override void Perform(Targetable target)
    {
        base.Perform(target);

        // display the trajectory
        lineRenderer.SetPosition(0, source.ProjecileSpawnPoint.position);
        lineRenderer.SetPosition(1, target.HitTargetPoint);
    }

    public override void Cast()
    {
        lineRenderer.positionCount = 0;    // remove lineRenderer points
        GameObject projectileGO = Instantiate(projectilePrefab, source.ProjecileSpawnPoint.position, source.ProjecileSpawnPoint.rotation);
        Projectile projectile = projectileGO.GetComponent<Projectile>();
        projectile.Damage = damageOnHit;
        projectile.TimeToLive = projectileTimeToLive;
    }
}
