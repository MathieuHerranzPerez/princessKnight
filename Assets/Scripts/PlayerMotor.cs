using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{

    // ---- INTERN ----
    private Rigidbody rb;
    private Vector3 velocity = Vector3.zero;
    private Vector3 movement = Vector3.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }


    void FixedUpdate()
    {
        PerformMovement();
    }


    public void Move(Vector3 direction, float speed)
    {
        // maybe should change that later for a system with rotation speed and going forward only
        // but there is maybe the simpliest and fastest way to move (mobile ready ;) )

        movement = direction * speed;
        // for the character face where he is going
        transform.LookAt(transform.position + direction);
    }


    private void PerformMovement()
    {
        // multiply by deltaTime to be sure to move the same way on every devices (slower or faster they are)
        rb.velocity = movement * Time.deltaTime * 20f;
    }
}
