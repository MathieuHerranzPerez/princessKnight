﻿using UnityEngine;

public class StatisticsManager : MonoBehaviour, IEventListener
{
    public static StatisticsManager Instance { get; private set; }

    public GameStats Stats { get { return gameStats; } }

    // ---- INTERN ----
    private GameStats gameStats = new GameStats();

    void Start()
    {
        Instance = this;
        EventManager.Instance.AddListener(this, EventName.EnemyDeath);
    }

    void OnDestroy()
    {
        EventManager.Instance.DetachListener(this, EventName.EnemyDeath);
    }

    public bool HandleEvent(IEvent evt)
    {
        bool res = false;
        switch(evt.GetName())
        {
            case EventName.EnemyDeath:
                Enemy enemy = (Enemy)evt.GetData();
                if (enemy is Boss)
                    ++gameStats.nbBossKilled;
                ++gameStats.nbEnemyKilled;

                if(gameStats.dictionaryMonster.ContainsKey(enemy.SpeciesName))
                {
                    gameStats.dictionaryMonster[enemy.SpeciesName] = gameStats.dictionaryMonster[enemy.SpeciesName] + 1;
                }
                else
                {
                    gameStats.dictionaryMonster.Add(enemy.SpeciesName, 1);
                }

                res = true;
                break;
        }

        return res;
    }
}
