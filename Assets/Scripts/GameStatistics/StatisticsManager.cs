using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsManager : MonoBehaviour
{
    public static StatisticsManager Instance { get; private set; }


    // ---- INTERN ----
    private GameStats gameStats = new GameStats();

    void Start()
    {
        Instance = this;
    }

    public void NotifyEnemyDeath(Enemy enemy)
    {
        ++gameStats.nbEnemyKilled;
    }
}
