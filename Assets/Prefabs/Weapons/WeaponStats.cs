
using UnityEngine;

[System.Serializable]
public class WeaponStats
{
    // player
    public Stats playerStats;

    // weapon
    [Range(0, 40)]
    public int defaultDamage = 10;
    public int damage = 10;

    public float defaultCouldownAttack = 1f;
    public float couldownAttack = 1f;
}
