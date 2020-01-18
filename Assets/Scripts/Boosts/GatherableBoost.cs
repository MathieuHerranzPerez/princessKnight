using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class GatherableBoost : MonoBehaviour
{
    [SerializeField] protected GameObject boostPrefab = default;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GiveBoost(other);
        }
    }

    protected abstract void GiveBoost(Collider other);
}
