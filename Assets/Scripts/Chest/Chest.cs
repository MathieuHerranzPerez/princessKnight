using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, Damageable
{
    [Range(1, 5)]
    [SerializeField] private int minHP = 1;
    [Range(1, 10)]
    [SerializeField] private int maxHP = 6;

    [Header("Setup")]
    [SerializeField] private Animator animator = default;

    // ---- INTERN ----
    private int HP = 1;
    private GameObject dropObject;

    void Awake()
    {
        HP = Random.Range(minHP, maxHP);
    }

    public void Init(GameObject prefabToDrop)
    {
        dropObject = prefabToDrop;
    }

    public void TakeDamage(int amount, DamageSource source)
    {
        if(source == DamageSource.PLAYER)
        {
            --HP;
            animator.SetTrigger("Hit");
            if (HP <= 0)
            {
                animator.SetTrigger("Open");
            }
        }
    }

    private void DestroySelf()
    {
        GameObject go = (GameObject)Instantiate(dropObject, transform.position, Quaternion.identity, transform.parent);
        Destroy(gameObject);
    }
}
