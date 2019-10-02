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
    private bool isAiming = false;


    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
    }

    protected override void Update()
    {
        base.Update();

        if (isAiming)
        {
            Vector3 dir = target.HitTargetPoint - source.ProjecileSpawnPoint.position;
            dir.Normalize();
            Vector3 point = dir * 20f + source.ProjecileSpawnPoint.position;

            lineRenderer.SetPosition(0, source.ProjecileSpawnPoint.position);
            lineRenderer.SetPosition(1, point);
        }
    }

    public override void Perform(Targetable target)
    {
        base.Perform(target);

        isAiming = true;
        // display the trajectory
        lineRenderer.positionCount = 2;
    }

    public override void Cast()
    {
        isAiming = false;
        lineRenderer.positionCount = 0;    // remove lineRenderer points
        GameObject projectileGO = Instantiate(projectilePrefab, source.ProjecileSpawnPoint.position, source.ProjecileSpawnPoint.rotation);
        Projectile projectile = projectileGO.GetComponent<Projectile>();
        projectile.Damage = damageOnHit;
        projectile.TimeToLive = projectileTimeToLive;
    }
}
