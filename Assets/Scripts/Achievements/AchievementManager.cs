using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance { get; private set; }

    public event Action<Achievement> OnAchievementCompleted;

    //[SerializeField] private AchievementDictionary achievementStorage = AchievementDictionary.New<AchievementDictionary>();
    //private Dictionary<string, Achievement> achievementDictionary
    //{
    //    get { return achievementStorage.dictionary; }
    //}
    [SerializeField] private AchievementDictionary achievementDictionary = new AchievementDictionary();

    void Awake()
    {
        if(Instance != null)
        {
            Debug.LogWarning("More than 1 AchievementManager in scene");
            Destroy(gameObject);
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public List<Achievement> GetAchievementList()
    {
        return achievementDictionary.Values.ToList();
    }

    public void UpdateAchievementWithGameStats(GameStats gameStats)
    {
        foreach (Achievement achievement in achievementDictionary.Values)
        {
            bool completed = achievement.CheckAchievementCompleted(gameStats);

            if(completed)
            {
                UnlockAchievement(achievement);
            }
        }
    }

    private void UnlockAchievement(Achievement achievement)
    {
        switch(achievement.RewardType)
        {
            case RewardType.CARD:
                MasterDeck.Instance.AddCardJustFoundToDeck(achievement.RewardGO.GetComponent<Card>());
                break;
            default:
                // nothing
                break;
        }

        OnAchievementCompleted?.Invoke(achievement);
    }
}
