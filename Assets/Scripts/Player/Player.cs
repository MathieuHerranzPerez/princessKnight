using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    public Transform HitTarget { get { return hitTarget; } }
    public Stats stats = default;

    [Header("Setup")]
    [SerializeField]
    private Transform hitTarget = default;

    // ---- INTERN ----
    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void TakeDamage(int amount)
    {
        playerController.TakeDamage(amount);
        // effects

    }
}
