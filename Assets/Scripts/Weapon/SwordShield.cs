using UnityEngine;

public class SwordShield : MeleeWeapon
{
    [SerializeField]
    private int maxCombo = 3;
    [SerializeField]
    private float timeToCombo = 3f;

    // ---- INERN ----
    private int nbCombo = 0;
    private float timeLeftToCombo = 0f;

    void Update()
    {
        if(timeLeftToCombo > 0f)
        {
            timeLeftToCombo -= Time.deltaTime;
        }
        else
        {
            nbCombo = 0;        // reset the combo
        }
    }

    /**
     * Obsolete
     */
    //public override void ActiveDefensiveColliders()
    //{
    //    arrayConcreteWeaponObject[1].SetColliderActive();
    //}

    //public override void ActiveOffensiveColliders()
    //{
    //    arrayConcreteWeaponObject[0].SetColliderActive();
    //}

    //public override void DesactiveDefensiveColliders()
    //{
    //    arrayConcreteWeaponObject[1].SetColliderInactive();
    //}

    //public override void DesactiveOffensiveColliders()
    //{
    //    arrayConcreteWeaponObject[0].SetColliderInactive();
    //}

    public override void ActiveDefensiveColliders()
    {
        throw new System.NotImplementedException();
    }

    public override void ActiveOffensiveColliders()
    {
        arrayConcreteWeaponObject[0].DisplayGFX();
        colliderAttackArray[nbCombo].ActiveCollider();
    }

    public override void DesactiveDefensiveColliders()
    {
        throw new System.NotImplementedException();
    }

    public override void DesactiveOffensiveColliders()
    {
        arrayConcreteWeaponObject[0].HideGFX();
        colliderAttackArray[nbCombo].DesactiveCollider();
    }

    public override void PerformAttack()
    {
        if (canAttack)
        {
            animator.SetTrigger("BasicAttack");
            animator.SetInteger("AttackNum", nbCombo);
            timeLeftToCombo = timeToCombo;

            nbCombo = nbCombo < maxCombo ? nbCombo + 1 : 0;

            canAttack = false;
            StartCoroutine("CountAttackCouldown");
        }
    }

    public override void PerformSpecial()
    {
        throw new System.NotImplementedException();
    }
}
