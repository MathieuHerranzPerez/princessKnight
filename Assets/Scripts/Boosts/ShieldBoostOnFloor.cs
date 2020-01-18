using UnityEngine;

public class ShieldBoostOnFloor : GatherableBoost
{

    protected override void GiveBoost(Collider other)
    {
        other.GetComponent<Player>().AddShield(boostPrefab);
        Destroy(gameObject);
    }
}
