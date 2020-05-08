using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private SceneFader sceneFader = default;
    //[SerializeField] private GameObject ScreenStatsPrefab = default;

    void Awake()
    {
        Instance = this;
    }

    public void NotifyPlayerDie()
    {
        AchievementManager.Instance.UpdateAchievementWithGameStats(StatisticsManager.Instance.Stats);
        TimeManager.Instance.Freeze();

        StatisticsScreen statisticsScreen = (StatisticsScreen) ScreenManager.Instance.ShowScreen(ScreenType.StatisticsScreen);

        //GameObject screenStatsGO = (GameObject) Instantiate(ScreenStatsPrefab);
        //StatisticsScreen statisticsScreen = screenStatsGO.GetComponent<StatisticsScreen>();
        statisticsScreen.Init(StatisticsManager.Instance.Stats);
    }

    public void NotifyStatsClose()
    {
        sceneFader.FadeTo("MathieuMenu");
    }
}
