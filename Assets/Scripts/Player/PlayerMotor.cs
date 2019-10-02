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

    private bool needToMove = false;
    private Vector3 direction = Vector3.zero;

    // dash
    private float dashTime;
    private Vector3 dashDirection;
    private float dashSpeed;
    // private Vector3 previousVelocity;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        direction = transform.forward;
    }

    void Update()
    {
        
    }


    void FixedUpdate()
    {
        PerformDash();
        PerformMovement();
    }


    public void Move(Vector3 direction, float speed)
    {
        movement = direction * speed;
        if (direction != Vector3.zero)
        {
            this.direction = direction;
        }
        needToMove = true;
    }

    public void Dash(float force, Vector3 direction, float time)
    {
        
        // previousVelocity = Vector3.zero;     // reset player velocity after dash
        dashDirection = direction;
        this.direction = dashDirection;
        dashSpeed = force;
        dashTime = time;
    }


    private void PerformMovement()
    {
        if (needToMove)
        {
            // maybe should change that later for a system with rotation speed and going forward only
            // but there is maybe the simpliest and fastest way to move (mobile ready ;) )

            // multiply by deltaTime to be sure to move the same way on every devices (slower or faster they are)
            // rb.velocity = movement * Time.deltaTime * 20f;
            transform.Translate(movement * Time.deltaTime, Space.World);



            // for the character face where he is going
            transform.LookAt(transform.position + direction);

            needToMove = false;
        }

        rb.angularVelocity = Vector3.zero;
    }

    private void PerformDash()
    {
        if (dashTime > 0)
        {
            needToMove = false;
            dashTime -= Time.deltaTime;
            // rb.velocity = dashDirection * dashSpeed;
            transform.Translate(dashDirection * dashSpeed * Time.deltaTime, Space.World);
            //if (dashTime <= 0)
            //{
            //   rb.velocity = previousVelocity;
            //}
            // for the character face where he is going
            transform.LookAt(transform.position + dashDirection);
        }
    }
}
