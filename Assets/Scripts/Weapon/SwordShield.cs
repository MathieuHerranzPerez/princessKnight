using UnityEngine;

public class SwordShield : MeleeWeapon
{
    [SerializeField]
    private int maxCombo = 3;
    [SerializeField]
    private float timeToCombo = 3f;

    [SerializeField] private ParticleSystem[] listParticleSystem = new ParticleSystem[1];

    // ---- INERN ----
    private int nbCombo = 0;
    private float timeLeftToCombo = 0f;
    private bool needToActiveOffensiveCollider = false;
    private bool previousFrameActiveOffensiveCollider = false;
    private int previousCombo = 0;

    protected override void Update()
    {
        base.Update();

        if(timeLeftToCombo > 0f)
        {
            timeLeftToCombo -= Time.deltaTime;
        }
        else
        {
            nbCombo = 0;        // reset the combo
        }

        if(needToActiveOffensiveCollider)
        {
            arrayConcreteWeaponObject[0].DisplayGFX();
            colliderAttackArray[nbCombo].ActiveCollider();
            previousFrameActiveOffensiveCollider = true;
            needToActiveOffensiveCollider = false;
            previousCombo = nbCombo;
            nbCombo = nbCombo < maxCombo ? nbCombo + 1 : 0;
        }
        else if(previousFrameActiveOffensiveCollider)
        {
            arrayConcreteWeaponObject[0].HideGFX();
            colliderAttackArray[previousCombo].DesactiveCollider();
        }
    }

    public override void ActiveOffensiveColliders()
    {
        needToActiveOffensiveCollider = true;
    }

    public override void DesactiveOffensiveColliders()
    {
        //arrayConcreteWeaponObject[0].HideGFX();
        //colliderAttackArray[nbCombo].DesactiveCollider();
    }

    public override bool PerformAttack()
    {
        bool res = false;
        if (canAttack)
        {
            animator.SetTrigger("BasicAttack");
            animator.SetInteger("AttackNum", nbCombo);
            timeLeftToCombo = timeToCombo;
            listParticleSystem[nbCombo].Play();

            canAttack = false;
            StartAttackCouldown();

            ActiveOffensiveColliders();

            res = true;
        }

        return res;
    }
}
