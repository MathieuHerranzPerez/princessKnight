using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MenuWeaponManager : MonoBehaviour
{
    public static MenuWeaponManager Instance;

    [SerializeField] List<string> listAnimationTriggerName = new List<string>();

    [Header("Setup")]
    [SerializeField] private Transform rightHandPoint = default;
    [SerializeField] private Transform leftHandPoint = default;

    [SerializeField] private GameObject firstWeaponPrefab = default;

    // ---- INTERN ----
    private GameObject currentWeaponPrefab;
    private Weapon currentWeapon;
    private Animator animator;

    private Dictionary<string, GameObject> listWeapon = new Dictionary<string, GameObject>();

    void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("More than 1 MenuWeaponManager in the scene");
        }
        Instance = this;
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        Invoke("ChangeAnimation", 5f);

        // put the default one
        ChangeWeapon(firstWeaponPrefab);
    }

    private void ChangeWeapon(GameObject newWeaponPrefab)
    {
        // remove the current weapon GO
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
            foreach (Transform child in rightHandPoint)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in leftHandPoint)
            {
                Destroy(child.gameObject);
            }
        }

        currentWeaponPrefab = newWeaponPrefab;
        InitCurrentWeapon();

        // TODO
        // notify something to start anim
    }

    public GameObject GetFirstWeaponPrefab()
    {
        return firstWeaponPrefab;
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

        return currentWeapon;
    }

    public void GiveNewWeapon(GameObject newWeaponPrefab)
    {
        Weapon newWeapon = newWeaponPrefab.GetComponent<Weapon>();
        if (!listWeapon.ContainsKey(newWeapon.NameLayerAnimator))
        {
            listWeapon.Add(newWeapon.NameLayerAnimator, newWeaponPrefab);
        }
        ChangeWeapon(newWeaponPrefab);
    }

    public void RemoveWeapon(GameObject weaponPrefab)
    {
        Weapon weapon = weaponPrefab.GetComponent<Weapon>();
        if (!listWeapon.ContainsKey(weapon.NameLayerAnimator))
        {
            return;
        }

        listWeapon.Remove(weaponPrefab.GetComponent<Weapon>().NameLayerAnimator);

        if(currentWeapon.NameLayerAnimator == weapon.NameLayerAnimator)
        {
            // check if there is another in the list
            if (listWeapon.Count > 0)
            {
                ChangeWeapon(listWeapon.Values.Last());
            }
            else {
                // else, put the default one
                ChangeWeapon(firstWeaponPrefab);
            }
        }
    }

    private void ChangeAnimation()
    {
        animator.SetTrigger(listAnimationTriggerName[Random.Range(0, listAnimationTriggerName.Count)]);
        Invoke("ChangeAnimation", Random.Range(10, 20));
    }
}
