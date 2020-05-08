using System.Collections.Generic;
using UnityEngine;

public abstract class Achievement : MonoBehaviour, Observable
{
    public string Title { get { return achievementInfos.title; } }
    public string Description { get { return achievementInfos.description; } }
    public int RewardPoint { get { return achievementInfos.rewardPoint; } }
    public bool Unlocked { get { return achievementInfos.unlocked; } }
    public Sprite Picture { get { return picture; } }
    public GameObject RewardGO { get { return rewardGO; } }
    public RewardType RewardType { get { return rewardType; } }

    public int Counter { get => achievementInfos.counter; protected set => achievementInfos.counter = value; }


    [SerializeField] protected RewardType rewardType = RewardType.CARD;
    [SerializeField] protected GameObject rewardGO = default;
    [SerializeField] protected AchievementInfos achievementInfos = default;

    [SerializeField] protected Sprite picture = default;

    [Header("Check completed")]
    [SerializeField] protected ComparisonOp comparisonOp = default;


    // ---- INTERN ----
    protected List<Observer> listObserver = new List<Observer>();

    public void Awake()
    {
        LoadAchievement();
    }

    public bool CheckAchievementCompleted(GameStats gameStats)
    {
        if (achievementInfos.unlocked)
            return false;

        int? gameStatsValue = GetValueFromGameStats(gameStats);
        int valueToCompareWith = UpdateLocalCounter(gameStatsValue);

        switch (comparisonOp)
        {
            case ComparisonOp.INF:
                return achievementInfos.counter < valueToCompareWith;

            case ComparisonOp.INF_EQL:
                return achievementInfos.counter <= valueToCompareWith;

            case ComparisonOp.EQL:
                return achievementInfos.counter == valueToCompareWith;

            case ComparisonOp.SUP_EQL:
                return achievementInfos.counter >= valueToCompareWith;

            case ComparisonOp.SUP:
                return achievementInfos.counter > valueToCompareWith;

            default:
                return false;
        }
    }

    public void Register(Observer obsever)
    {
        listObserver.Add(obsever);
    }

    protected abstract int? GetValueFromGameStats(GameStats gameStats);


    protected void LoadAchievement()
    {
        this.achievementInfos = SaveSystem.Load<AchievementInfos>(achievementInfos.title);
    }

    protected int UpdateLocalCounter(int? value)
    {
        Debug.LogError("UpdateLocalCounter : " + value); // affD

        if(value != null)
        {
            achievementInfos.counter += value ?? 0;
            // save the new value
            SaveSystem.Save<AchievementInfos>(achievementInfos, achievementInfos.title);
        }
        return achievementInfos.counter;
    }

}

public enum RewardType
{
    CARD,
    SKIN
}

public enum ComparisonOp
{
    INF,
    INF_EQL,
    EQL,
    SUP_EQL,
    SUP,
}