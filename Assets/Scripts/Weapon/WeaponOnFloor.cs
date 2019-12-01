using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WeaponOnFloor : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField]
    private GameObject WeaponPrefab = default;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<WeaponManager>().ChangeWeapon(WeaponPrefab);
            Destroy(gameObject);
        }
    }
}
