using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour, Observer
{
    [Header("Setup")]
    [SerializeField]
    private Player player;
    [Header("UI")]
    [SerializeField]
    private Image healthAmountImage;

    void Start()
    {
        player.Register(this);
        Notify();
    }

    public void Notify()
    {
        healthAmountImage.fillAmount = (float)player.stats.HP / (float)player.stats.maxHP;
    }
}
