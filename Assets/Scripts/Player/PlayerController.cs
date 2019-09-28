using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(WeaponManager))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Stats stats = default;

    [Header("Setup")]
    [SerializeField]
    private PlayerInputs playerInputs = default;

    // ---- INTERN ----
    private PlayerMotor motor;
    private WeaponManager weaponManager;
    private Animator animator;

    private Vector3 direction = Vector3.zero;
    private Vector3 dashDirection = Vector3.zero;

    private bool canDash = true;


    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        weaponManager = GetComponent<WeaponManager>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // get the player inputs
        float directionX = playerInputs.Horizontal;
        float directionY = playerInputs.Verical;

        // move
        direction = new Vector3(directionX, 0f, directionY);
        motor.Move(direction, stats.speed);

        // dash
        dashDirection = playerInputs.Swipe;
        if (canDash && dashDirection != Vector3.zero)
        {
            canDash = false;
            motor.Dash(stats.dashSpeed, dashDirection, stats.dashTime);
            animator.SetTrigger("Dash");
            StartCoroutine("countDashCouldown");
        }

        // attack
        bool wantToAttack = playerInputs.A;
        // todo
    }

    private IEnumerator countDashCouldown()
    {
        float time = stats.dashCouldown;
        while(time > 0f)
        {
            time -= Time.deltaTime;
            yield return null;;
        }
        canDash = true;
    }
}
