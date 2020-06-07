using System.Collections.Generic;
using UnityEngine;

public class GameManager : ResetableManager
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private SceneFader sceneFader = default;

    protected override void Awake()
    {
        Instance = this;
    }

    public void NotifyPlayerDie()
    {
        if (AchievementManager.Instance != null)
        {
            AchievementManager.Instance.UpdateAchievementWithGameStats(StatisticsManager.Instance.Stats);
        }
        TimeManager.Instance.ChangeTimeScale(0.2f);

        ScoreManager.Instance.SaveScore();

        StatisticsScreen statisticsScreen = (StatisticsScreen) ScreenManager.Instance.ShowScreen(ScreenType.StatisticsScreen);
        statisticsScreen.Init(StatisticsManager.Instance.Stats);

        ResetScene();
    }

    public void NotifyStatsClose()
    {
        sceneFader.FadeTo("MathieuMenu");
    }


    public override void ResetScene()
    {
        foreach(ResetableManager manager in listAllManager)
        {
            manager.ResetScene();
        }
    }
}
