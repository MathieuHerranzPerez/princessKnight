using System;
using UnityEngine;

[Serializable]
public class AchievementInfos
{
    public string title;
    public string description;
    public int rewardPoint;
    [HideInInspector]
    public bool unlocked = false;
    [HideInInspector]
    public int counter;
}
