using System;
using UnityEngine;

[Serializable]
public class Reward
{
    public Achievement Achievement { get { return achievement; } }
    public RewardType RewardType { get { return rewardType; } }
    public GameObject RewardGO { get { return rewardGO; } }

    [SerializeField] private Achievement achievement = default;
    [SerializeField] private RewardType rewardType = RewardType.CARD;
    [SerializeField] private GameObject rewardGO = default;
}

public enum RewardType
{
    CARD,
    SKIN
}