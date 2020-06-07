using System;

[Serializable]
public class AchievementInfos
{
    public string title;
    public string description;
    public int rewardPoint;

    public bool unlocked = false;

    public int counter;

    public override string ToString()
    {
        return "[" + title + "] " + description;
    }
}
