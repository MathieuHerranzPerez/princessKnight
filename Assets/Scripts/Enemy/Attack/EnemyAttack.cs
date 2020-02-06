using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour
{
    public float Range { get { return range; } }
    public int Damage { get { return damageOnHit; } }
    public Enemy Source { set { source = value; } } 
    public Targetable Target { set { target = value; } }
    public float Couldown { get { return currentCouldown; } }

    [SerializeField]
    protected int damageOnHit = 1;
    [SerializeField]
    protected float range = 2f;
    [SerializeField]
    protected float couldown = 3f;

    // TODO
    //[Header("GFX")]
    //[SerializeField]
    //protected ParticleSystem hitParticle = default;

    // ---- INERN ----
    protected Targetable target;
    protected Enemy source;
    protected float currentCouldown = 0f;

    protected virtual void Update()
    {
        if(currentCouldown > 0f)
        {
            currentCouldown -= Time.deltaTime;
        }
    }

    /**
     * call when you need to launch the attack
     */
    public virtual void Perform(Targetable target)
    {
        // stop the anim during the attaque animation
        // don't forget to Unfreeze with anim event at the end of the animation
        currentCouldown = 5f;
        source.FreezeMovement();
        source.Anim.SetTrigger("Attack");
    }

    /**
     * call when you have to instantiate a projectile, make an AOE or make direct damage
     */ 
    public abstract void Cast();

    /**
     * call at the end of attack animation
     */
    public virtual void End()
    {
        currentCouldown = couldown;
        source.Unfreeze();
    }

    public virtual void StopEffects()
    {

    }
}
