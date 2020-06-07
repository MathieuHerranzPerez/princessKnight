using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyGroupGOFactory))]
public class MapManager : ResetableManager
{
    public static MapManager Instance { get; private set; }

    [Range(0f, 1f)]
    [SerializeField] private float chanceToSpawnChest = 0.2f;
    [Range(1, 3)]
    [SerializeField] private int nbMaxChestPerFragment = 2;

    [Header("Setup")]
    [SerializeField] private int nbMapFragmentToCheckPoint = 4;
    [SerializeField] private Transform mapFragmentContainer = default;
    [SerializeField] private int mapFragmentSize = 30;
    [SerializeField] private string prefabMapFragmentFolder = "";
    [SerializeField] private string[] listPrefabMapFragmentSubFolder = new string[1];

    [Header("Boss")]
    [SerializeField] private int nbCheckpointForBoss = 2;
    [SerializeField] private string prefabBossMapFragmentFolder = "";

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
    private int nbBossGenerated = 0;

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
        else if((nbMapFragmentGenerated + 1) % nbMapFragmentToCheckPoint == 0 && (nbCheckpointMapFragment + 1) % nbCheckpointForBoss == 0)
        {
            ChargeNewBossMap();
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

    private void AddOneToMapFragmentFolder()
    {
        ++currentMapFragmentFolderIndex;
        if (currentMapFragmentFolderIndex == listPrefabMapFragmentFolderFullPath.Length)
        {
            currentMapFragmentFolderIndex = 0;
        }
    }

    private void ChargeFirstsMap()
    {
        // the the first map fragment
        SpawnFirstMapFragment();

        currentMapFragmentFolderIndex = 0;
        mapFragmentSelection = Resources.LoadAll(listPrefabMapFragmentFolderFullPath[currentMapFragmentFolderIndex], typeof(GameObject));
        GameObject mapFragmentToSpawn = (GameObject)mapFragmentSelection[Random.Range(0, mapFragmentSelection.Length - 1)];

        SpawnMap(mapFragmentToSpawn, false);

        // charge the following
        ChargeNewMap();
    }

    private void ChargeNewMap()
    {
        GameObject mapFragmentToSpawn;

        // charge the following map fragments
        mapFragmentSelection = Resources.LoadAll(listPrefabMapFragmentFolderFullPath[currentMapFragmentFolderIndex], typeof(GameObject));
        mapFragmentToSpawn = (GameObject)mapFragmentSelection[Random.Range(0, mapFragmentSelection.Length - 1)];
        SpawnMap(mapFragmentToSpawn, false);

        AddOneToMapFragmentFolder();
    }

    private void ChargeNewBossMap()
    {
        GameObject mapFragmentToSpawn;

        // charge the following map fragments
        mapFragmentSelection = Resources.LoadAll(prefabBossMapFragmentFolder, typeof(GameObject));
        mapFragmentToSpawn = (GameObject)mapFragmentSelection[Random.Range(0, mapFragmentSelection.Length - 1)];
        SpawnMap(mapFragmentToSpawn, true);

        AddOneToMapFragmentFolder();
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

    private void SpawnMap(GameObject mapFragmentToSpawn, bool isBossMap)
    {
        Debug.Log("Spawn map");
        float distance = (nbMapFragmentGenerated) * mapFragmentSize + offsetFragment + nbCheckpointMapFragment * checkpointMapFragmentSize;
        Vector3 pos = mapFragmentContainer.forward * distance;

        GameObject mapFragmentGO = Instantiate(mapFragmentToSpawn, pos, mapFragmentContainer.rotation, mapFragmentContainer);
        
        ++nbMapFragmentGenerated;

        // if we need to spawn a checkpoint as next map
        needToSpawnCheckPoint = ((nbMapFragmentGenerated % nbMapFragmentToCheckPoint) == 0 && nbMapFragmentGenerated > 0);

        // instantiate enemies, princes...

        int nbEnemyToSpawn = isBossMap ? 0 : 7;
        int nbPrinceToSpawn = isBossMap ? 0 : 7;

        // TODO need better than that v1.0
        GameObject[] arrayEnemyGroupGO = new GameObject[nbEnemyToSpawn];
        GameObject[] arrayPrinceGO = new GameObject[nbPrinceToSpawn];
        for(int i = 0; i < nbEnemyToSpawn; ++i)
        {
            arrayEnemyGroupGO[i] = enemyGroupGOFactory.GetEnemyGroupGO(nbMapFragmentGenerated);
        }
        for (int i = 0; i < nbPrinceToSpawn; ++i)
        {
            arrayPrinceGO[i] = princePrefab;
        }


        int nbChestToSpawn = 0;
        for(int i = 0; i < nbMaxChestPerFragment; ++i)
        {
            float random = Random.Range(0f, 1f);
            if (random < (isBossMap ? 1.5 * chanceToSpawnChest : chanceToSpawnChest))
            {
                ++nbChestToSpawn;
            }
        }

        if (isBossMap)
        {
            ++nbBossGenerated;
            // get a random boss
            GameObject bossGO = enemyGroupGOFactory.GetBossGO(nbBossGenerated);

            BossRoom bossRoom = mapFragmentGO.GetComponent<BossRoom>();
            queueMapFragment.Enqueue(bossRoom);
            bossRoom.InitWith(arrayEnemyGroupGO, arrayPrinceGO, nbChestToSpawn, bossGO);
        }
        else
        {
            LevelMapFragment levelMapFragment = mapFragmentGO.GetComponent<LevelMapFragment>();
            queueMapFragment.Enqueue(levelMapFragment);
            levelMapFragment.InitWith(arrayEnemyGroupGO, arrayPrinceGO, nbChestToSpawn);
        }
    }

    public override void ResetScene()
    {
        foreach(Transform child in mapFragmentContainer)
        {
            Destroy(child.gameObject);
        }
    }
}
