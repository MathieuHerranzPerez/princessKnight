using BeautifulTransitions.Scripts.Transitions.Components.GameObject.AbstractClasses;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementNotification : PopupNotification
{
    [SerializeField] private Image image = default;
    [SerializeField] private TransitionGameObjectBase checkTransition = default;
    [SerializeField] private TextMeshProUGUI title = default;
    [SerializeField] private TextMeshProUGUI desc = default;

    public void Init(Achievement achievement)
    {
        image.sprite = achievement.Picture;
        title.text = achievement.Title;
        desc.text = achievement.Description;

        Invoke("FadeCheck", 1f);
    }

    private void FadeCheck()
    {
        checkTransition.TransitionIn();
    }
}
