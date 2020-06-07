using UnityEngine;

public class HerdManager : ResetableManager
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

    public override void ResetScene()
    {
        Destroy(herd.gameObject);
    }
}
