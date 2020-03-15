﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatisticsScreen : MonoBehaviour
{
    [SerializeField] private Text textScore = default;
    [SerializeField] private Text textDistance = default;
    [SerializeField] private Text textNbEnemiesKilled = default;
    [SerializeField] private Text textNbBossKilled = default;

    // ---- INTERN ----
    private GameStats gameStats;

    public void Init(GameStats gameStats)
    {
        this.gameStats = gameStats;
        InitTexts();
    }

    public void Close()
    {
        GameManager.Instance.NotifyStatsClose();
    }

    private void InitTexts()
    {
        textScore.text = ScoreManager.Instance.Score.ToString();
        textDistance.text = ((int) MapManager.Instance.GetMaxDistanceDiscovered()).ToString();
        textNbEnemiesKilled.text = gameStats.nbEnemyKilled.ToString();
        textNbBossKilled.text = gameStats.nbBossKilled.ToString();
    }
}
