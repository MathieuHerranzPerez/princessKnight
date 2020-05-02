using System.Collections.Generic;
using UnityEngine;

public class AchievementScreen : BaseScreen
{
    [Header("Setup")]
    [SerializeField] private GameObject achievementGFXPrefab = default;
    [SerializeField] protected Transform achievementContainer = default;

    protected override void OnStart()
    {
        DisplayAchievements(AchievementManager.Instance.GetAchievementList());
        AchievementManager.Instance.OnAchievementCompleted += HandleAchievementCompleted;
    }

    private void DisplayAchievements(List<Achievement> listAchievement)
    {
        foreach (Achievement achievement in listAchievement)
        {
            CreateAchievementGFX(achievement, achievementContainer);
        }
    }

    private void CreateAchievementGFX(Achievement achievement, Transform achievementGFXContainer)
    {
        GameObject achievementGFXGO = (GameObject)Instantiate(achievementGFXPrefab, achievementGFXContainer);
        AchievementGFX achievementGFX = achievementGFXGO.GetComponent<AchievementGFX>();

        achievementGFX.SetAchievement(achievement);
    }

    private void HandleAchievementCompleted(Achievement achievement)
    {
        // todo OPTI find better way
        DisplayAchievements(AchievementManager.Instance.GetAchievementList());
    }

    void OnDestroy()
    {
        AchievementManager.Instance.OnAchievementCompleted -= HandleAchievementCompleted;
    }
}
