using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyGroupGOFactory))]
public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    [Range(0f, 1f)]
    [SerializeField] private float chanceToSpawnChest = 0.2f;
    [Range(1, 3)]
    [SerializeField] private int nbMaxChestPerFragment = 2;

    [Header("Setup")]
    [SerializeField]
    private int nbMapFragmentToCheckPoint = 4;
    [SerializeField]
    private Transform mapFragmentContainer = default;
    [SerializeField]
    private int mapFragmentSize = 30;
    [SerializeField]
    private string prefabMapFragmentFolder = "";
    [SerializeField]
    private string[] listPrefabMapFragmentSubFolder = new string[1];

    [Header("First map fragment")]
    [SerializeField]
    private int firstMapFragmentSize = 15;
    [SerializeField]
    private GameObject firstMapFragmentPrefab = default;

    [Header("Checkpoint map fragment")]
    [SerializeField]
    private GameObject[] listPrefabCheckpointMapFragment = default;
    [SerializeField]
    private int checkpointMapFragmentSize = 10;

    [Header("Prince")]
    [SerializeField]
    private GameObject princePrefab = default;

    // ---- INTERN ----
    private string[] listPrefabMapFragmentFolderFullPath;
    private Queue<MapFragment> queueMapFragment = new Queue<MapFragment>();
    private int currentMapFragmentFolderIndex = 0;
    private Object[] mapFragmentSelection;                              // store the mapFragment Object loaded from Ressources folder 
    private int nbMapFragmentGenerated = 0;
    private int nbCheckpointMapFragment = 0;
    private int nbEndOfMap = 0;
    private float offsetFragment = 0;
    private float offsetCheckpoint = 0;
    private bool needToSpawnCheckPoint = false;

    private EnemyGroupGOFactory enemyGroupGOFactory;

    void Start()
    {
        Instance = this;

        offsetFragment = mapFragmentSize / 2f + firstMapFragmentSize / 2f;
        offsetCheckpoint = checkpointMapFragmentSize / 2f + firstMapFragmentSize / 2f;

        enemyGroupGOFactory = GetComponent<EnemyGroupGOFactory>();

        // initialize the folder names to not have doing it all the time while instantiating new map fragments
        listPrefabMapFragmentFolderFullPath = new string[listPrefabMapFragmentSubFolder.Length];
        for (int i = 0; i < listPrefabMapFragmentSubFolder.Length; ++i)
        {
            listPrefabMapFragmentFolderFullPath[i] = prefabMapFragmentFolder + "/" + listPrefabMapFragmentSubFolder[i];
        }

        ChargeFirstsMap();
    }

    /**
     * Just need to call it to charge a new map fragment and remove the last one
     */ 
    public void NotifyEndOfMap()
    {
        ++nbEndOfMap;
        if (nbEndOfMap > 1 && nbEndOfMap % 4 != 0)
        {
            // remove the last map
            MapFragment firstMapFragment = queueMapFragment.Dequeue();
            firstMapFragment.Destroy();

            if(nbEndOfMap != 2 && nbEndOfMap % 4 == 2)
            {
                // remove the last map
                firstMapFragment = queueMapFragment.Dequeue();
                firstMapFragment.Destroy();
            }
        }

        // instantiate a map
        if(needToSpawnCheckPoint)
        {
            SpawnCheckpointMap();
        }
        else
        {
            ChargeNewMap();
        }
    }

    public float GetMaxDistanceDiscovered()
    {
        int nbMapFragmentDiscovered = nbMapFragmentGenerated;
        int nbCheckpointDiscovered = nbCheckpointMapFragment;

        for(int i = 0; i < 2; ++i)
        {
            if(nbMapFragmentDiscovered % 4 == 0 && nbMapFragmentDiscovered / 4 == nbCheckpointDiscovered)
            {
                --nbCheckpointDiscovered;
            }
            else
            {
                --nbMapFragmentDiscovered;
            }
        }

        return nbMapFragmentDiscovered * mapFragmentSize + nbCheckpointDiscovered * checkpointMapFragmentSize;
    }

    private void ChargeFirstsMap()
    {
        // the the first map fragment
        SpawnFirstMapFragment();

        currentMapFragmentFolderIndex = 0;
        mapFragmentSelection = Resources.LoadAll(listPrefabMapFragmentFolderFullPath[currentMapFragmentFolderIndex], typeof(GameObject));
        GameObject mapFragmentToSpawn = (GameObject)mapFragmentSelection[Random.Range(0, mapFragmentSelection.Length - 1)];

        SpawnMap(mapFragmentToSpawn);

        // charge the following
        ChargeNewMap();
    }

    private void ChargeNewMap()
    {
        GameObject mapFragmentToSpawn;

        // charge the following map fragments
        mapFragmentSelection = Resources.LoadAll(listPrefabMapFragmentFolderFullPath[currentMapFragmentFolderIndex], typeof(GameObject));
        mapFragmentToSpawn = (GameObject)mapFragmentSelection[Random.Range(0, mapFragmentSelection.Length - 1)];
        SpawnMap(mapFragmentToSpawn);


        ++currentMapFragmentFolderIndex;
        if (currentMapFragmentFolderIndex == listPrefabMapFragmentFolderFullPath.Length)
        {
            currentMapFragmentFolderIndex = 0;
        }
    }

    private void SpawnFirstMapFragment()
    {
        Vector3 pos = new Vector3(0f, 0f, -0f);
        GameObject mapFragmentGO = Instantiate(firstMapFragmentPrefab, pos, mapFragmentContainer.rotation, mapFragmentContainer);
        MapFragment mapFragment = mapFragmentGO.GetComponent<MapFragment>();
        queueMapFragment.Enqueue(mapFragment);
    }

    private void SpawnCheckpointMap()
    {
        needToSpawnCheckPoint = false;
        float distance = (nbMapFragmentGenerated) * mapFragmentSize + offsetCheckpoint + nbCheckpointMapFragment * checkpointMapFragmentSize;
        Vector3 pos = mapFragmentContainer.forward * distance;
        GameObject mapFragmentGO = Instantiate(listPrefabCheckpointMapFragment[Random.Range(0, listPrefabCheckpointMapFragment.Length-1)], pos, mapFragmentContainer.rotation, mapFragmentContainer);
        MapFragment mapFragment = mapFragmentGO.GetComponent<MapFragment>();
        queueMapFragment.Enqueue(mapFragment);

        ++nbCheckpointMapFragment;
    }

    private void SpawnMap(GameObject mapFragmentToSpawn)
    {
        Debug.Log("Spawn map");
        float distance = (nbMapFragmentGenerated) * mapFragmentSize + offsetFragment + nbCheckpointMapFragment * checkpointMapFragmentSize;
        Vector3 pos = mapFragmentContainer.forward * distance;
        GameObject mapFragmentGO = Instantiate(mapFragmentToSpawn, pos, mapFragmentContainer.rotation, mapFragmentContainer);
        LevelMapFragment levelMapFragment = mapFragmentGO.GetComponent<LevelMapFragment>();
        queueMapFragment.Enqueue(levelMapFragment);
 
        ++nbMapFragmentGenerated;

        // if we need to spawn a checkpoint as next map
        needToSpawnCheckPoint = ((nbMapFragmentGenerated % nbMapFragmentToCheckPoint) == 0 && nbMapFragmentGenerated > 0);

        // instantiate enemies, princes...

        // TODO need better than that v1.0
        GameObject[] arrayEnemyGroupGO = new GameObject[7];
        GameObject[] arrayPrinceGO = new GameObject[5];
        for(int i = 0; i < 7; ++i)
        {
            arrayEnemyGroupGO[i] = enemyGroupGOFactory.GetEnemyGroupGO(nbMapFragmentGenerated);
        }
        for (int i = 0; i < 5; ++i)
        {
            arrayPrinceGO[i] = princePrefab;
        }


        int nbChestToSpawn = 0;
        for(int i = 0; i < nbMaxChestPerFragment; ++i)
        {
            float random = Random.Range(0f, 1f);
            if (random < chanceToSpawnChest)
            {
                ++nbChestToSpawn;
            }
        }

        levelMapFragment.InitWith(arrayEnemyGroupGO, arrayPrinceGO, nbChestToSpawn);
    }
}
