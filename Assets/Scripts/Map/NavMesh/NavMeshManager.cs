using UnityEngine;

public class NavMeshManager : MonoBehaviour
{
    public static NavMeshManager Instance { get; private set; }
    public string[] ListTag { get { return listNavMeshAgentTag; } }

    [SerializeField]
    private string[] listNavMeshAgentTag = new string[1];

    void Awake()
    {
        Instance = this;
    }
}
