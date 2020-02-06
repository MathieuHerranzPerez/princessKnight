using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponSpell : MonoBehaviour, Observable
{
    public Sprite Sprite { get { return sprite; } }
    public float TotalCouldown { get { return couldown; } }
    public float CurrentCouldown { get { return time; } }

    [Range(0.5f, 40f)]
    [SerializeField] protected float couldown = 10f;

    [Header("Setup")]
    [SerializeField] protected Sprite sprite = default;
    [SerializeField] protected ParticleSystem spellEffect = default;

    // ---- INTERN ----
    protected float time = 0f;
    protected bool canUseSpell = true;
    protected bool isUsingSpell = false;
    protected bool wasUsingSpellLastFrame = false;

    protected Weapon weapon;
    protected Animator animator;

    private List<Observer> listObserver = new List<Observer>();

    protected virtual void Update()
    {
        if(!canUseSpell)
        {
            time -= Time.deltaTime;

            if(time <= 0f)
            {
                time = 0f;
                canUseSpell = true;
            }
            else
            {

            }

            NotifyObservers();
        }
    }

    public void Init(Animator animator, Weapon weapon)
    {
        this.weapon = weapon;
        this.animator = animator;
    }

    public void Register(Observer obsever)
    {
        listObserver.Add(obsever);
    }

    public void UseEffect()
    {
        if (canUseSpell)
        {
            time = couldown;
            canUseSpell = false;
            animator.SetTrigger("SpellAttack");
        }
    }

    public virtual void PerformEffect()
    {
        isUsingSpell = true;
        ParticleSystem effect = (ParticleSystem) Instantiate(spellEffect, transform.position, transform.rotation, transform);
        Destroy(effect.gameObject, 2f);
    }

    protected void NotifyObservers()
    {
        foreach(Observer o in listObserver)
        {
            o.Notify();
        }
    }
}
