using System.Collections.Generic;
using UnityEngine;

public class MapFragment : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField]
    private SpawnChunck[] arrayEnemyAndPrinceSpawnPoint = new SpawnChunck[1];
    [SerializeField]
    private NavMeshObject navMeshObject = default;
    [SerializeField]
    private Transform objectContainer = default;    // every object in the mapFragment must be instantiated in this transform
                                                    // to be desroyed when fragment will be. To not destroying object, just put
                                                    // its transform to null (root)
    [SerializeField]
    private Transform checkpointSpawnPoint = default;

    // ---- INTERN ----
    private List<int> listAllIndex = new List<int>();

    void Awake()
    {
        // a list that contains all the index of the array
        for (int i = 0; i < arrayEnemyAndPrinceSpawnPoint.Length; ++i)
        {
            listAllIndex.Add(i);
        }
    }

    public void Destroy()
    {
        // TODO remove all content


        Destroy(transform.gameObject);
    }

    public void InitWith(GameObject[] arrayEnemyGO, GameObject[] arrayPrinceGO, bool needToPutCheckPoint)
    {
        // navMeshObject.BuildNavMesh();
        SpawnEnemiesAndPrinces(arrayEnemyGO, arrayPrinceGO);

        if(needToPutCheckPoint)
        {
            Instantiate(MapManager.Instance.CheckpointPrefab, checkpointSpawnPoint);
        }
    }

    // TODO pooling
    private void SpawnEnemiesAndPrinces(GameObject[] arrayEnemyGO, GameObject[] arrayPrinceGO)
    {
        List<int> listIndexNotUsed = listAllIndex;

        for (int i = 0; i < arrayEnemyGO.Length || i < arrayPrinceGO.Length; ++i)
        {
            int randomIndex = listIndexNotUsed[Random.Range(0, listIndexNotUsed.Count)];

            if (i < arrayEnemyGO.Length)
            {
                Instantiate(arrayEnemyGO[i], arrayEnemyAndPrinceSpawnPoint[randomIndex].enemySpawnPoint.position, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f), objectContainer);
            }

            if (i < arrayPrinceGO.Length)
            {
                Instantiate(arrayPrinceGO[i], arrayEnemyAndPrinceSpawnPoint[randomIndex].princeSpawnPoint.position, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f), objectContainer);
            }

            listIndexNotUsed.Remove(randomIndex);
        }
    }
}

[System.Serializable]
public class SpawnChunck
{
    public Transform enemySpawnPoint;
    public Transform princeSpawnPoint;
}