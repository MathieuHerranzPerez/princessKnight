using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour, Observable
{
    public static ScoreManager Instance { get; private set; }

    public float Score { get { return score; } }
    public bool IsNewecord { get { return isNewRecord; } }

    [SerializeField]
    private float princeDeahImpactOnScore = -1f;
    [SerializeField]
    private float princeSavedImpactOnScore = 3f;

    // ---- INTERN ----
    private bool isNewRecord = false;
    private float record = 0f;
    private float score = 0f;

    private List<Observer> listObserver = new List<Observer>();

    void Start()
    {
        Instance = this;
        record = PlayerPrefs.GetFloat("scoreRecord", 0);
    }

    public void NotifyPrinceDeath()
    {
        score += princeDeahImpactOnScore;
        NotifyObservers();
    }

    public void NotifyPrinceSaved()
    {
        score += princeSavedImpactOnScore;
        NotifyObservers();
    }

    public void SaveScore()
    {
        if(score > record)
        {
            isNewRecord = true;
            PlayerPrefs.SetFloat("scoreRecord", score);
        }
    }


    // pattern observer
    public void Register(Observer obsever)
    {
        listObserver.Add(obsever);
    }
    // pattern observer
    private void NotifyObservers()
    {
        foreach (Observer o in listObserver)
        {
            o.Notify();
        }
    }
}
