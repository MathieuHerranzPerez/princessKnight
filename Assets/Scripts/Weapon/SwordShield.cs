using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordShield : MeleeWeapon
{

    public override void ActiveDefensiveColliders()
    {
        arrayConcreteWeaponObject[1].SetColliderActive();
    }

    public override void ActiveOffensiveColliders()
    {
        arrayConcreteWeaponObject[0].SetColliderActive();
    }

    public override void DesactiveDefensiveColliders()
    {
        arrayConcreteWeaponObject[1].SetColliderInactive();
    }

    public override void DesactiveOffensiveColliders()
    {
        arrayConcreteWeaponObject[0].SetColliderInactive();
    }

    public override void PerformAttack()
    {
        animator.SetTrigger("BasicAttack");
    }

    public override void PerformSpecial()
    {
        throw new System.NotImplementedException();
    }
}
