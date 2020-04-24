using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance { get; private set; }

    [Header("Setup")]
    [SerializeField] private GameObject achievementGFXPrefab = default;
    [SerializeField] private Transform achievementsContainer = default;
    [SerializeField] private AchievementDictionary achievementStorage = AchievementDictionary.New<AchievementDictionary>();
    private Dictionary<string, Achievement> achievementDictionary
    {
        get { return achievementStorage.dictionary; }
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

        Achievement achievement = GetAchievementFromTitle(title);
        achievementGFX.SetAchievement(achievement);
    }



    private Achievement GetAchievementFromTitle(string title)
    {
        if (!achievementDictionary.ContainsKey(title))
            return null;

        return achievementDictionary[title];
    }
}
