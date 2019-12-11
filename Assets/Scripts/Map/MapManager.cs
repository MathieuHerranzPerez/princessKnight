using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyGroupGOFactory))]
public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }
    
    public GameObject CheckpointPrefab { get { return checkpointPrefab; } }

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
    [SerializeField]
    private GameObject princePrefab = default;
    [SerializeField]
    private GameObject checkpointPrefab = default;

    // ---- INTERN ----
    private string[] listPrefabMapFragmentFolderFullPath;
    private Queue<MapFragment> queueMapFragment = new Queue<MapFragment>();
    private int currentMapFragmentFolderIndex = 0;
    private Object[] mapFragmentSelection;                              // store the mapFragment Object loaded from Ressources folder 
    private int nbMapFragmentGenerated = 0;
    private int nbEndOfMap = 0;

    private EnemyGroupGOFactory enemyGroupGOFactory;

    void Start()
    {
        Instance = this;

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
        if (nbEndOfMap > 1)
        {
            // remove the last map
            MapFragment firstMapFragment = queueMapFragment.Dequeue();
            firstMapFragment.Destroy();
        }

        // instantiate a map
        ChargeNewMap();
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
        // float distance = -firstMapFragmentSize;
        Vector3 pos = new Vector3(0f, 0f, -0f);//mapFragmentContainer.forward * distance;
        GameObject mapFragmentGO = Instantiate(firstMapFragmentPrefab, pos, mapFragmentContainer.rotation, mapFragmentContainer);
        MapFragment mapFragment = mapFragmentGO.GetComponent<MapFragment>();
        queueMapFragment.Enqueue(mapFragment);
    }

    private void SpawnMap(GameObject mapFragmentToSpawn)
    {
        Debug.Log("Spawn map");
        float distance = (nbMapFragmentGenerated) * mapFragmentSize + mapFragmentSize / 2f + firstMapFragmentSize / 2f;
        Vector3 pos = mapFragmentContainer.forward * distance;
        GameObject mapFragmentGO = Instantiate(mapFragmentToSpawn, pos, mapFragmentContainer.rotation, mapFragmentContainer);
        MapFragment mapFragment = mapFragmentGO.GetComponent<MapFragment>();
        queueMapFragment.Enqueue(mapFragment);

        // if we need to spawn a checkpoint
        bool needToSpawnCheckPoint = ((nbMapFragmentGenerated % nbMapFragmentToCheckPoint) == 0 && nbMapFragmentGenerated > 0);
 
        ++nbMapFragmentGenerated;

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

        mapFragment.InitWith(arrayEnemyGroupGO, arrayPrinceGO, needToSpawnCheckPoint);
    }
}
