using UnityEngine;

public abstract class Targetable : MonoBehaviour
{
    public Vector3 HitTargetPoint { get { return hitTarget.position; } }

    [Header("Setup")]
    [SerializeField]
    private Transform hitTarget = default;

    public abstract void TakeDamage(int amount);
}
