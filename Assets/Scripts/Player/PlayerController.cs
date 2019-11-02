using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(WeaponManager))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{

    [Header("Setup")]
    [SerializeField]
    private PlayerInputs playerInputs = default;
    [SerializeField]
    private GameObject firstWeaponPrefab = default;

    [Header("GFX")]
    [SerializeField]
    private ParticleSystem dashStartParticles = default;


    // ---- INTERN ----
    private PlayerMotor motor;
    private WeaponManager weaponManager;
    private Animator animator;
    private Player player;

    private Vector3 direction = Vector3.zero;
    private Vector3 dashDirection = Vector3.zero;

    private bool canDash = true;
    private float dashCouldown = 0f;


    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        weaponManager = GetComponent<WeaponManager>();
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();

        GetNewWeapon(firstWeaponPrefab);
    }

    void Update()
    {
        // get the player inputs
        float directionX = playerInputs.Horizontal;
        float directionY = playerInputs.Verical;

        // move
        direction = new Vector3(directionX, 0f, directionY);
        // I didn't found better way that using magnitude :/
        float maginitude = direction.magnitude;
        animator.SetFloat("Speed", maginitude);
        motor.Move(direction, player.stats.speed * maginitude);


        // dash
        if(!canDash)
        {
            dashCouldown -= Time.deltaTime;
            if(dashCouldown <= 0f)
            {
                canDash = true;
            }
        }
        dashDirection = playerInputs.Swipe;
        if (canDash && dashDirection != Vector3.zero)
        {
            canDash = false;
            motor.Dash(player.stats.dashSpeed, dashDirection, player.stats.dashTime);
            animator.SetTrigger("Dash");
            dashCouldown = player.stats.dashCouldown;
            // StartCoroutine("CountDashCouldown");

            // GFX
            // calculate rotation between current pos and dashDirection
            float yAngle = Vector3.SignedAngle(dashDirection, Vector3.forward, -Vector3.up);
            Quaternion rot = Quaternion.Euler(0f, yAngle, 0f);
            ParticleSystem ps = (ParticleSystem)Instantiate(dashStartParticles, transform.position, rot);
            Destroy(ps.gameObject, 0.8f);
        }

        // attack
        bool wantToAttack = playerInputs.A;
        if (wantToAttack)
        {
            weaponManager.PerformWeaponAttack();
        }
    }

    // when the player need to change his weapon
    public void GetNewWeapon(GameObject weaponPefab)
    {
        WeaponStats ws = weaponManager.ChangeWeapon(firstWeaponPrefab);
        player.stats.ChangeStats(ws.speed, ws.dashSpeed, ws.dashTime, ws.dashCouldown);
    }

    private IEnumerator CountDashCouldown()
    {
        float time = player.stats.dashCouldown;
        while(time > 0f)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        Debug.Log("no couldown");
        canDash = true;
    }
}
