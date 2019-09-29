
using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    public int maxHP = 5;
    public int HP = 5;

    [HideInInspector]
    public float attackCouldown = 3f;
    public float specialCouldown = 10f;

    [HideInInspector]
    public float defaultSpeed = 10f;
    [HideInInspector]
    public float speed = 10f;

    [HideInInspector]
    public float dashSpeed = 10f;
    [HideInInspector]
    public float dashTime = 0.4f;
    [HideInInspector]
    public float dashCouldown = 2f;


    public void ChangeStats(float attackCouldown, float speed, float dashSpeed, float dashTime, float dashCouldown)
    {
        defaultSpeed = speed;
        this.speed = speed;

        this.attackCouldown = attackCouldown;
        this.dashSpeed = dashSpeed;
        this.dashTime = dashTime;
        this.dashCouldown = dashCouldown;
    }
}
