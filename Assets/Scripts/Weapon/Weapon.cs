using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public bool IsOneHand { get { return arrayWeaponObject.Length == 1;  } }
    public GameObject RightHandWeapon { get { return arrayWeaponObject[0]; } }
    public GameObject LeftHandWeapon { get { return IsOneHand ? null : arrayWeaponObject[1]; } }


    // TODO some stats


    [Header("Setup")]
    [SerializeField]
    protected RuntimeAnimatorController animatorController = default;
    [SerializeField]
    protected GameObject[] arrayWeaponObject = new GameObject[1];           // 1 or 2 weapons

    // ---- INTERN ---
    protected WeaponObject[] arrayConcreteWeaponObject;
    protected Animator animator;

    public void SetWeaponsObject(WeaponObject[] arrayWeaponObject)
    {
        arrayConcreteWeaponObject = new WeaponObject[arrayWeaponObject.Length];
        for (int i = 0; i < arrayWeaponObject.Length; ++i)
        {
            arrayConcreteWeaponObject[i] = arrayWeaponObject[i];
        }
    }

    public void SetAnimator(Animator animator)
    {
        this.animator = animator;
    }

    public abstract void PerformAttack();
    public abstract void PerformSpecial();
    public abstract void ActiveOffensiveColliders();
    public abstract void DesactiveOffensiveColliders();
    public abstract void ActiveDefensiveColliders();
    public abstract void DesactiveDefensiveColliders();
}
