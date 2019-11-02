using UnityEngine;
using UnityEngine.AI;

public class NavMeshObject : MonoBehaviour
{
    private static NavMeshSurface currentGlobalNavMesh;

    [SerializeField]
    private NavMeshSurface navMeshSurface;


    //[SerializeField]
    //private NavMeshData navMeshData;
    //private NavMeshDataInstance navMeshInstance;

    public void BuildNavMesh()
    {
        if (currentGlobalNavMesh)
            Destroy(currentGlobalNavMesh);

        navMeshSurface.BuildNavMesh();
        currentGlobalNavMesh = navMeshSurface;
    }

    //void OnEnable()
    //{
    //    navMeshData.rotation = this.transform.rotation;
    //    navMeshData.position = this.transform.position;
    //    navMeshInstance = NavMesh.AddNavMeshData(navMeshData);
    //}

    //void OnDisable()
    //{
    //    NavMesh.RemoveAllNavMeshData(); ;
    //}
}
