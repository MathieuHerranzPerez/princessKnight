using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class ColliderAttackDef : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField]
    protected Weapon weapon = default;

    // ---- INTERN ----
    protected Collider col;

    void Start()
    {
        col = GetComponent<Collider>();
        DesactiveCollider();
    }

    public void ActiveCollider()
    {
        col.enabled = true;
    }

    public void DesactiveCollider()
    {
        col.enabled = false;
    }

    protected abstract void ActOnTriggerEnter(Collider other);

    void OnTriggerEnter(Collider other)
    {
        ActOnTriggerEnter(other);
    }
}
