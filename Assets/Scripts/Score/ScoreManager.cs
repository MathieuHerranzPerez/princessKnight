using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour, Observable
{
    public static ScoreManager Instance { get; private set; }

    public float Score { get { return score; } }

    [SerializeField]
    private float princeDeahImpactOnScore = -1f;
    [SerializeField]
    private float princeSavedImpactOnScore = 3f;

    // ---- INTERN ----
    private float score = 0f;

    private List<Observer> listObserver = new List<Observer>();

    void Start()
    {
        Instance = this;
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
