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
    private float timeToLive;
    private Rigidbody rb;

    void Start()
    {
        Destroy(this.gameObject, timeToLive);
        rb = GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(0f, 0f, speed), ForceMode.Impulse);
    }

}
