using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Stats stats = default;

    [Header("Setup")]
    [SerializeField]
    private Joystick joystick = default;

    // ---- INTERN ----
    private PlayerMotor motor;


    // Start is called before the first frame update
    void Start()
    {
        motor = GetComponent<PlayerMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
        direction.Normalize();
        motor.Move(direction, stats.speed);
    }
}
