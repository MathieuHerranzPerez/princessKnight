using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    private Weapon currentWeapon = default;

    [Header("Setup")]
    [SerializeField]
    private Transform rightHandPoint = default;
    [SerializeField]
    private Transform leftHandPoint = default;

    // ---- INTERN ----
    

    void Start()
    {
        InitCurrentWeapon();
    }

    public void PerformWeaponAttack()
    {
        currentWeapon.PerformAttack();
    }

    public void PerformWeaponSpecial()
    {
        currentWeapon.PerformSpecial();
    }

    public void ActiveOffensiveColliders()
    {
        currentWeapon.ActiveOffensiveColliders();
    }

    public void DesactiveOffensiveColliders()
    {
        currentWeapon.DesactiveOffensiveColliders();
    }

    public void ActiveDefensiveCollider()
    {
        currentWeapon.ActiveDefensiveColliders();
    }

    public void DesactiveDefensiveColiders()
    {
        currentWeapon.DesactiveDefensiveColliders();
    }

    private void InitCurrentWeapon()
    {
        int nbWeapon = 1;
        // create weapon object in scene
        GameObject weapon1 = (GameObject) Instantiate(currentWeapon.RightHandWeapon, rightHandPoint);
        GameObject weapon2 = null;
        if (!currentWeapon.IsOneHand)
        {
            ++nbWeapon;
            weapon2 = (GameObject) Instantiate(currentWeapon.LeftHandWeapon, leftHandPoint);
        }

        GameObject[] arrayWeaponGO = new GameObject[nbWeapon];
        arrayWeaponGO[0] = weapon1;
        if(nbWeapon > 1)
        {
            arrayWeaponGO[1] = weapon2;
        }

        WeaponObject[] arrayWeaponObject = new WeaponObject[nbWeapon];
        for(int i = 0; i < nbWeapon; ++i)
        {
            arrayWeaponObject[i] = arrayWeaponGO[i].GetComponent<WeaponObject>();
        }

        currentWeapon.SetWeaponsObject(arrayWeaponObject);
    }

    private void ChangeWeapon(Weapon newWeapon)
    {
        currentWeapon = newWeapon;
        InitCurrentWeapon();
    }
}
