using System.Collections.Generic;
using UnityEngine;

public class LevelMapFragment : MapFragment
{
    [Header("Setup")]
    [SerializeField] protected SpawnChunck[] arrayEnemyAndPrinceSpawnPoint = new SpawnChunck[1];
    [SerializeField] protected Transform[] arrayChestSpawnPoint = new Transform[1];
    [SerializeField] protected NavMeshObject navMeshObject = default;
    [SerializeField] protected Transform objectContainer = default;    // every object in the mapFragment must be instantiated in this transform
                                                                       // to be desroyed when fragment will be. To not destroying object, just put
                                                                       // its transform to null (root)

    // ---- INTERN ----
    protected List<int> listAllIndex = new List<int>();

    protected void Awake()
    {
        // a list that contains all the index of the array
        for (int i = 0; i < arrayEnemyAndPrinceSpawnPoint.Length; ++i)
        {
            listAllIndex.Add(i);
        }
    }

    public void InitWith(GameObject[] arrayEnemyGO, GameObject[] arrayPrinceGO, int nbChest)
    {
        // navMeshObject.BuildNavMesh();
        SpawnEnemiesAndPrinces(arrayEnemyGO, arrayPrinceGO);
        SpawnChest(nbChest);
    }

    // TODO pooling
    protected void SpawnEnemiesAndPrinces(GameObject[] arrayEnemyGO, GameObject[] arrayPrinceGO)
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

    protected void SpawnChest(int nbChestToSpawn)
    {
        nbChestToSpawn = Mathf.Min(nbChestToSpawn, arrayChestSpawnPoint.Length);
        List<int> indexList = new List<int>();
        for(int i = 0; i < arrayChestSpawnPoint.Length; ++i)
        {
            indexList.Add(i);
        }
        for (int i = 0; i < nbChestToSpawn; ++i)
        {
            int index = indexList[Random.Range(0, indexList.Count -1)];
            ChestManager.Instance.SpawnChest(arrayChestSpawnPoint[index]);
            indexList.Remove(index);
        }
    }
}

[System.Serializable]
public class SpawnChunck
{
    public Transform enemySpawnPoint;
    public Transform princeSpawnPoint;
}
