using UnityEngine;

public class TimeManager : ResetableManager
{
    public static TimeManager Instance { get; private set; }

    private float previousTimeScale = 1f;

    protected override void Awake()
    {
        base.Awake();
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

    public override void ResetScene()
    {
        Time.timeScale = 1f;
        UnFreeze();
    }
}
