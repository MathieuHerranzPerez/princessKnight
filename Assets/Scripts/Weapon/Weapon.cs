using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public bool IsOneHand { get { return arrayWeaponObject.Length == 1;  } }
    public GameObject RightHandWeapon { get { return arrayWeaponObject[0]; } }
    public GameObject LeftHandWeapon { get { return IsOneHand ? null : arrayWeaponObject[1]; } }
    public string NameLayerAnimator { get { return nameLayerAnimator; } }
    public WeaponSpell[] WeaponSpells { get { return this.arrayWeaponSpell; } }

    public WeaponStats stats;

    [Header("Setup")]
    [SerializeField]
    protected string nameLayerAnimator = "";
    [SerializeField] protected WeaponSpell[] arrayWeaponSpell = new WeaponSpell[1];


    [Header("[0]: attack, [1]: def")]
    [SerializeField]
    protected ColliderAttack[] colliderAttackArray = default;
    [SerializeField]
    protected ColliderDef colliderDef = default;
    [SerializeField]
    protected GameObject[] arrayWeaponObject = new GameObject[1];           // 1 or 2 weapons

    // ---- INTERN ---
    protected WeaponGFXObject[] arrayConcreteWeaponObject;
    protected Animator animator;

    protected float timeCouldown = 0f;

    protected bool canAttack = true;

    protected virtual void Update()
    {
        if(timeCouldown > 0f)
        {
            timeCouldown -= Time.deltaTime;
            if(timeCouldown <= 0f)
            {
                canAttack = true;
            }
        }
    }

    public void SetWeaponsObject(WeaponGFXObject[] arrayWeaponObject)
    {
        arrayConcreteWeaponObject = new WeaponGFXObject[arrayWeaponObject.Length];
        for (int i = 0; i < arrayWeaponObject.Length; ++i)
        {
            arrayConcreteWeaponObject[i] = arrayWeaponObject[i];
        }
    }

    public void SetAnimator(Animator animator)
    {
        this.animator = animator;
        foreach (WeaponSpell weaponSpell in arrayWeaponSpell)
        {
            weaponSpell.Init(animator, this);
        }
    }

    public abstract void PerformAttack();
    public abstract void ActiveOffensiveColliders();
    public abstract void DesactiveOffensiveColliders();

    protected void StartAttackCouldown()
    {
        timeCouldown = stats.attackCouldown;
    }

    //protected IEnumerator CountAttackCouldown()
    //{
    //    float time = stats.attackCouldown;
    //    while(time > 0f)
    //    {
    //        time -= Time.deltaTime;
    //        yield return null;
    //    }

    //    canAttack = true;
    //}
}
