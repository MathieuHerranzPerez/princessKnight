using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    private float previousTimeScale = 1f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Time.timeScale = 1f;
    }

    public void Freeze()
    {
        previousTimeScale = Time.timeScale > 0.01f ? Time.timeScale : 1f;
        Time.timeScale = 0f;
    }

    public void UnFreeze()
    {
        Time.timeScale = previousTimeScale;
    }

    public void ChangeTimeScale(float newTimeScale)
    {
        Time.timeScale = newTimeScale;
        previousTimeScale = newTimeScale;
    }
}
