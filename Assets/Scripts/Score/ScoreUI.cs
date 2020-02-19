using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour, Observer
{
    [Header("Setup")]
    [SerializeField] private ScoreManager scoreManager = default;
    [Header("UI")]
    [SerializeField] private Text scoreText = default;

    void Start()
    {
        scoreManager.Register(this);
        Notify();
    }

    public void Notify()
    {
        scoreText.text = scoreManager.Score.ToString();
    }
}
