using UnityEngine;

public class HerdManager : MonoBehaviour
{
    public static HerdManager Instance { get; private set; }
    public Herd Herd { get { return herd; } }

    [Header("Setup")]
    [SerializeField]
    private Herd herd = default;

    void Start()
    {
        Instance = this;
    }
}
