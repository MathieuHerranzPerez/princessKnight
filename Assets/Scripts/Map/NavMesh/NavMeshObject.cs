using UnityEngine;
using UnityEngine.AI;

public class NavMeshObject : MonoBehaviour
{
    [SerializeField]
    private NavMeshData navMeshData;
    private NavMeshDataInstance navMeshInstance;

    void OnEnable()
    {
        navMeshData.rotation = this.transform.rotation;
        navMeshData.position = this.transform.position;
        navMeshInstance = NavMesh.AddNavMeshData(navMeshData);
    }

    void OnDisable()
    {
        NavMesh.RemoveNavMeshData(navMeshInstance);
    }
}
