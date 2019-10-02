using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public int Damage { get { return damage; } set { damage = value; } }
    public float TimeToLive { set { timeToLive = value; } }

    [SerializeField]
    private float speed = 15f;


    // ---- INTERN ----
    private int damage;
    private float timeToLive = 2f;
    private Rigidbody rb;

    void Start()
    {
        Destroy(this.gameObject, timeToLive);
        rb = GetComponent<Rigidbody>();
        Vector3 force = transform.forward * speed;
        rb.AddForce(force, ForceMode.Impulse);
    }

}
