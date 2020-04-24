using System.Collections.Generic;
using UnityEngine;

public class Achievement : MonoBehaviour, Observable
{
    public string Title { get { return achievementInfos.title; } }
    public string Description { get { return achievementInfos.description; } }
    public int RewardPoint { get { return achievementInfos.rewardPoint; } }
    public bool Unlocked { get { return achievementInfos.unlocked; } }
    public Sprite Picture { get { return picture; } }

    protected int Counter { get => achievementInfos.counter; set => achievementInfos.counter = value; }
    public string GameStatsAttributeCheckEarn { get => gameStatsAttributeCheckEarn; set => gameStatsAttributeCheckEarn = value; }

    [SerializeField] private RewardType rewardType = RewardType.CARD;
    [SerializeField] private GameObject rewardGO = default;
    [SerializeField] private AchievementInfos achievementInfos = default;

    [SerializeField] private Sprite picture = default;

    [Header("Check completed")]
    [SerializeField] private ComparisonOp comparisonOp = default;


    // ---- INTERN ----

    private string gameStatsAttributeCheckEarn;
    private List<Observer> listObserver = new List<Observer>();

    public bool CheckAchievementCompleted(GameStats gameStats)
    {
        int? gameStatsValue = (typeof(GameStats).GetField(gameStatsAttributeCheckEarn).GetValue(gameStats)) as int?;
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

    private int UpdateLocalCounter(int? value)
    {
        if(value != null)
        {
            achievementInfos.counter += value ?? 0;
            // save the new value
            // TODO
        }
        return achievementInfos.counter += value ?? 0;
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