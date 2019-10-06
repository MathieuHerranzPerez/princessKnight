using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WeaponObject : MonoBehaviour
{
    [SerializeField]
    protected GameObject effectOnColliderActive = default;
    // it contains all informations about a physic weapon

    // ---- INTERN ----
    private Collider wpCollider;
    protected Weapon weapon;

    void Start()
    {
        wpCollider = GetComponent<Collider>();
    }

    public void SetWeapon(Weapon weapon)
    {
        this.weapon = weapon;
    }


    public void SetColliderActive()
    {
        wpCollider.enabled = true;
        effectOnColliderActive.SetActive(true);
    }

    public void SetColliderInactive()
    {
        wpCollider.enabled = false;
        effectOnColliderActive.SetActive(false);
    }
}
