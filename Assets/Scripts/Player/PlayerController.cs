using System.Collections;
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
    [SerializeField] private LayerMask enemyLayerMask = default;

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
            bool isAttacking = weaponManager.PerformWeaponAttack();

            if(isAttacking)
            {
                Vector3 direction;
                // rotate the player to face the nearest enemy
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5, enemyLayerMask);
                if (hitColliders.Length > 0) {
                    Collider nearest = hitColliders[0];
                    int i = 1;
                    float bestDistanceSquared = Mathf.Pow(transform.position.x - nearest.transform.position.x, 2) + Mathf.Pow(transform.position.z - nearest.transform.position.z, 2);
                    while (i < hitColliders.Length)
                    {
                        Vector3 pos = hitColliders[i].transform.position;
                        float distanceSquared = Mathf.Pow(transform.position.x - pos.x, 2) + Mathf.Pow(transform.position.z - pos.z, 2);
                        if(distanceSquared < bestDistanceSquared)
                        {
                            bestDistanceSquared = distanceSquared;
                            nearest = hitColliders[i];
                        }
                        ++i;
                    }
                    direction = new Vector3(nearest.transform.position.x - transform.position.x, 0f, nearest.transform.position.z - transform.position.z);
                    direction.Normalize();
                }
                else
                {
                    direction = transform.forward;
                }
                motor.Dash(player.stats.dashSpeedWhenAttack, direction, 0.1f);
                //motor.Move(direction * 5f, player.stats.speed);
            }
        }
    }

    // when the player need to change his weapon
    public void GetNewWeapon(GameObject weaponPefab)
    {
        WeaponStats ws = weaponManager.ChangeWeapon(firstWeaponPrefab);
        player.stats.ChangeStats(ws.speed, ws.dashSpeed, ws.dashTime, ws.dashCouldown, ws.dashSpeedWhenAttack);
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
