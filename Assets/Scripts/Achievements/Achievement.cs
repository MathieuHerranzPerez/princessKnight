using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Achievement : Observable
{
    public string Title { get { return title; } }
    public string Description { get { return description; } }
    public int RewardPoint { get { return rewardPoint; } }
    public bool Unlocked { get { return unlocked; } }
    public Sprite Picture { get { return picture; } }

    protected int Counter { get => counter; set => counter = value; }

    [SerializeField] protected string title;
    [SerializeField] protected string description;
    [SerializeField] protected int rewardPoint;
    [SerializeField] protected bool unlocked = false;
    [SerializeField] private int counter;

    [SerializeField] protected Sprite picture = default;
    [SerializeField] protected Reward reward;

    private List<Observer> listObserver = new List<Observer>();
    
    //public Achievement(string title, string description, int rewardPoint, bool unlocked, int counter)
    //{
    //    this.listObserver = new List<Observer>();

    //    this.title = title;
    //    this.description = description;
    //    this.rewardPoint = rewardPoint;
    //    this.unlocked = unlocked;
    //    this.counter = counter;
    //}

    public abstract bool CheckAchievementEarn(GameStats gameStats);

    public void Register(Observer obsever)
    {
        listObserver.Add(obsever);
    }
}
