using UnityEngine;

public class WeaponGFXObject : MonoBehaviour
{
    //[SerializeField]
    //protected GameObject effectOnColliderActive = default;
    // it contains all informations about a physic weapon

    // ---- INTERN ----
    protected Weapon weapon;


    public void SetWeapon(Weapon weapon)
    {
        this.weapon = weapon;
    }

    public void DisplayGFX()
    {
        //effectOnColliderActive.SetActive(true);
    }

    public void HideGFX()
    {
        //effectOnColliderActive.SetActive(false);
    }
}
