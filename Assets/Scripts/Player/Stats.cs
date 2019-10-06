
using UnityEngine;

[System.Serializable]
public class Stats
{
    public int maxHP = 5;
    public int HP = 5;

    public float attackCouldown = 3f;
    public float specialCouldown = 10f;

    // change with weapon
    public float defaultSpeed = 10f;
    public float speed = 10f;

    public float dashSpeed = 10f;
    public float dashTime = 0.4f;
    public float dashCouldown = 2f;

    // enemy only
    [Header("Enemy only")]
    public float aggroRange = 10f;

    public void ChangeStats(float speed, float dashSpeed, float dashTime, float dashCouldown)
    {
        defaultSpeed = speed;
        this.speed = speed;

        this.dashSpeed = dashSpeed;
        this.dashTime = dashTime;
        this.dashCouldown = dashCouldown;
    }
}
