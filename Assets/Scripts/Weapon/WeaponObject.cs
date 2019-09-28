using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WeaponObject : MonoBehaviour
{
    // it contains all informations about a physic weapon

    // ---- INTERN ----
    private Collider wpCollider;
    protected Weapon weapon;
    private HingeJoint joint;

    void Start()
    {
        wpCollider = GetComponent<Collider>();
        joint = GetComponent<HingeJoint>();
    }

    public void SetWeapon(Weapon weapon)
    {
        this.weapon = weapon;
    }


    public void SetColliderActive()
    {
        wpCollider.enabled = true;
    }

    public void SetColliderInactive()
    {
        wpCollider.enabled = false;
    }
}
