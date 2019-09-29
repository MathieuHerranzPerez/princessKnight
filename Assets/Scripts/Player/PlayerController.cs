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
    [SerializeField]
    private GameObject firstWeaponPrefab = default;

    [Header("GFX")]
    [SerializeField]
    private ParticleSystem dashStartParticles = default;


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

        GetNewWeapon(firstWeaponPrefab);
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
        Debug.Log(dashDirection);
        if (canDash && dashDirection != Vector3.zero)
        {
            canDash = false;
            motor.Dash(stats.dashSpeed, dashDirection, stats.dashTime);
            animator.SetTrigger("Dash");
            StartCoroutine("CountDashCouldown");

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

    public void TakeDamage(int amount)
    {
        CameraShake.Instance.Shake(0.15f, 0.3f, 0.15f);
        animator.SetTrigger("TakingDamage");
        StartCoroutine("CountCouldownSpeedWhenTakingDamage");
        stats.HP -= amount;
        if(stats.HP <= 0)
        {
            Die();
        }
    }

    public void GetNewWeapon(GameObject weaponPefab)
    {
        WeaponStats ws = weaponManager.ChangeWeapon(firstWeaponPrefab);
        stats.ChangeStats(ws.playerStats.speed, ws.playerStats.dashSpeed, ws.playerStats.dashTime, ws.playerStats.dashCouldown);
    }

    private void Die()
    {
        // todo
        Debug.Log("DIE DIE DIE !");
    }

    private IEnumerator CountCouldownSpeedWhenTakingDamage()
    {
        stats.speed = 0f;
        float time = 0.2f;
        while(time > 0f)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        stats.speed = stats.defaultSpeed;
    }

    private IEnumerator CountDashCouldown()
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
