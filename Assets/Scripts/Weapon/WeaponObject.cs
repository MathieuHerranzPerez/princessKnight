using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WeaponObject : MonoBehaviour
{
    // it contains all informations about a physic weapon

    // ---- INTERN ----
    private Collider wpCollider;

    void Awake()
    {
        wpCollider = GetComponent<Collider>();
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
