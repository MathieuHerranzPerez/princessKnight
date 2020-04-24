using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementGFX : MonoBehaviour, Observer
{
    [Header("Setup")]
    [SerializeField] private TextMeshProUGUI titleText = default;
    [SerializeField] private TextMeshProUGUI descriptionText = default;
    [SerializeField] private Image picture = default;

    [SerializeField] private Image lockImage = default;
    [SerializeField] private Sprite lockSprite = default;
    [SerializeField] private Sprite unlockedSprite = default;

    // ---- INTERN ----
    private Achievement achievement;

    public void SetAchievement(Achievement achievement)
    {
        this.achievement = achievement;

        achievement.Register(this);

        titleText.text = achievement.Title;
        descriptionText.text = achievement.Description;
        this.picture.sprite = achievement.Picture;

        this.lockImage.sprite = achievement.Unlocked ? unlockedSprite : lockSprite;
    }


    public void Notify()
    {
        this.lockImage.sprite = achievement.Unlocked ? unlockedSprite : lockSprite;
    }
}
