using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WeaponManager : MonoBehaviour
{

    [Header("Setup")]
    [SerializeField]
    private Transform rightHandPoint = default;
    [SerializeField]
    private Transform leftHandPoint = default;

    [SerializeField] private GameObject firstWeaponPrefab = default;

    [SerializeField] private SpellUI spellUI = default;

    // ---- INTERN ----
    private GameObject currentWeaponPrefab;
    private Weapon currentWeapon;
    private Animator animator;


    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public bool PerformWeaponAttack()
    {
        return currentWeapon.PerformAttack();
    }

    public void ActiveOffensiveColliders()
    {
        currentWeapon.ActiveOffensiveColliders();
    }

    public void DesactiveOffensiveColliders()
    {
        currentWeapon.DesactiveOffensiveColliders();
    }

    public void PerformSpellEffect(int num)
    {
        currentWeapon.WeaponSpells[num].PerformEffect();
    }

    public WeaponStats ChangeWeapon(GameObject newWeaponPrefab)
    {
        currentWeaponPrefab = newWeaponPrefab;
        return InitCurrentWeapon().stats;
    }

    public GameObject GetFirstWeaponPrefab()
    {
        string weaponPrefabName = PlayerPrefs.GetString("firstWeapon");
        GameObject weaponPrefab;

        weaponPrefab = (GameObject)Resources.Load("Weapons/" + weaponPrefabName);
        if (weaponPrefab == null)
        {
            Debug.LogWarning("no weapon in ressource folder or playerPrefs");
            weaponPrefab = firstWeaponPrefab;
        }

        return weaponPrefab;
    }

    private Weapon InitCurrentWeapon()
    {
        GameObject weaponGO = (GameObject)Instantiate(currentWeaponPrefab, transform);
        currentWeapon = weaponGO.GetComponent<Weapon>();

        int nbWeapon = 1;
        // create weapon object in scene
        GameObject weapon1 = (GameObject)Instantiate(currentWeapon.RightHandWeapon, rightHandPoint);
        GameObject weapon2 = null;
        if (!currentWeapon.IsOneHand)
        {
            ++nbWeapon;
            weapon2 = (GameObject)Instantiate(currentWeapon.LeftHandWeapon, leftHandPoint);
        }

        GameObject[] arrayWeaponGO = new GameObject[nbWeapon];
        arrayWeaponGO[0] = weapon1;
        if (nbWeapon > 1)
        {
            arrayWeaponGO[1] = weapon2;
        }

        WeaponGFXObject[] arrayWeaponObject = new WeaponGFXObject[nbWeapon];
        for (int i = 0; i < nbWeapon; ++i)
        {
            arrayWeaponObject[i] = arrayWeaponGO[i].GetComponent<WeaponGFXObject>();
            arrayWeaponObject[i].SetWeapon(currentWeapon);
        }

        currentWeapon.SetWeaponsObject(arrayWeaponObject);
        currentWeapon.SetAnimator(animator);

        animator.SetLayerWeight(animator.GetLayerIndex(currentWeapon.NameLayerAnimator), 1f);
        // animator.runtimeAnimatorController = currentWeapon.AnimatorController;

        // the spell UI
        spellUI.InitWithWeaponSpell(currentWeapon.WeaponSpells);

        return currentWeapon;
    }
}
