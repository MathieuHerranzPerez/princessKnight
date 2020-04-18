using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance { get; private set; }

    [Header("Setup")]
    [SerializeField] private GameObject achievementGFXPrefab = default;
    [SerializeField] private Transform achievementsContainer = default;

    [SerializeField] private RewardDictionary rewardStorage = RewardDictionary.New<RewardDictionary>();
    private Dictionary<string, Reward> rewardDictionary
    {
        get { return rewardStorage.dictionary; }
    }

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogWarning("More than 1 AchievementManager in scene");
        }
        Instance = this;
    }

    public void CreateAchievementGFX(string title)
    {
        GameObject achievementGFXGO = (GameObject)Instantiate(achievementGFXPrefab, achievementsContainer);
        AchievementGFX achievementGFX = achievementGFXGO.GetComponent<AchievementGFX>();

        Achievement achievement = GetRewardFromTitle(title).Achievement;
        achievementGFX.SetAchievement(achievement);
    }



    private Reward GetRewardFromTitle(string title)
    {
        if (!rewardDictionary.ContainsKey(title))
            return null;

        return rewardDictionary[title];
    }
}
