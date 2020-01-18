using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Animator))]
public class Player : Targetable, Observable
{
    public Stats stats = default;

    [Header("Setup")]
    [SerializeField] private Transform boostContainer = default;

    // ---- INTERN ----
    private PlayerController playerController;
    private Animator animator;

    private List<Observer> listObserver = new List<Observer>();

    private Stack<ShieldBoost> stackShieldBoost = new Stack<ShieldBoost>();

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }


    public override void TakeDamage(int amount, DamageSource source)
    {
        if (stackShieldBoost.Count == 0)
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
        else
        {
            ShieldBoost shieldBoost = stackShieldBoost.Pop();
            shieldBoost.Remove();
        }
    }

    public void AddShield(GameObject shieldBoostGO)
    {
        GameObject shieldGO = (GameObject)Instantiate(shieldBoostGO, boostContainer);
        shieldGO.transform.localPosition = Vector3.zero;
        ShieldBoost shieldBoost = shieldGO.GetComponent<ShieldBoost>();
        stackShieldBoost.Push(shieldBoost);
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

