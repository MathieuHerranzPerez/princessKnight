using UnityEngine;

public class PlayerVision : MonoBehaviour
{
    public static int InBushViewRange { get; private set; }

    [SerializeField]
    private int viewRadiusInBushes = 5;

    void Awake()
    {
        InBushViewRange = viewRadiusInBushes;
    }
}
