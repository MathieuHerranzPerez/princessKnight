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
    [SerializeField] private Transform achievementContainer = default;
    [SerializeField] private AchievementDictionary achievementDictionary = new AchievementDictionary();

    // ---- INTERN ----
    private AchievementDictionary inGameAchievementDictionary = new AchievementDictionary();

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

    private void Start()
    {
        foreach(KeyValuePair<string, Achievement> element in achievementDictionary)
        {
            Achievement achievement = Instantiate(element.Value, achievementContainer);
            inGameAchievementDictionary.Add(achievement.Title, achievement);
        }
    }

    public List<Achievement> GetAchievementList()
    {
        return inGameAchievementDictionary.Values.ToList();
    }

    public void UpdateAchievementWithGameStats(GameStats gameStats)
    {
        Debug.Log(gameStats); // affD
        foreach (Achievement achievement in inGameAchievementDictionary.Values)
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
        achievement.Unlock();
        switch (achievement.RewardType)
        {
            case RewardType.CARD:
                MasterDeck.Instance.AddCardJustFoundToDeck(achievement.RewardGO.GetComponent<Card>());
                break;
            default:
                // nothing
                break;
        }

        OnAchievementCompleted?.Invoke(achievement);

        AchievementNotification achievementNotification = ScreenManager.Instance.CreateNotificationPopup<AchievementNotification>();
        if(achievementNotification != null)
        {
            achievementNotification.Init(achievement);
        }
    }
}
