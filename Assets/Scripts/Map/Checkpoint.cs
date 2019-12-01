using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Checkpoint : MonoBehaviour
{
    public Vector3 Position { get { return pointToDestroyAgent.position; } }
    public bool IsActive { get { return isActive; } }

    [Header("Setup")]
    [SerializeField]
    private Transform pointToDestroyAgent = default;

    // ---- INTERN ----
    private bool isActive = false;
    private Collider col;

    void Start()
    {
        col = GetComponent<Collider>();
        col.enabled = false;
    }

    public void Active()
    {
        col.enabled = true;
        isActive = true;
    }
}
