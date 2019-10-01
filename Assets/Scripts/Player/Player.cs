using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Animator))]
public class Player : Targetable
{
    public Stats stats = default;

    // ---- INTERN ----
    private PlayerController playerController;
    private Animator animator;

    private List<Observer> listObserver = new List<Observer>();

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }


    public override void TakeDamage(int amount)
    {
        CameraShake.Instance.Shake(0.10f, 0.15f, 0.10f);
        animator.SetTrigger("TakingDamage");
        stats.HP -= amount;

        NotifyObservers();

        if (stats.HP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // todo
        Debug.Log("DIE DIE DIE !");
    }




    /**
     * Observer / observable pattern
     */
    public void Register(Observer obsever)
    {
        listObserver.Add(obsever);
    }

    private void NotifyObservers()
    {
        foreach (Observer o in listObserver)
        {
            o.Notify();
        }
    }
}

