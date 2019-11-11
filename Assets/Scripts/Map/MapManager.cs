using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyGroupGOFactory))]
public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }
    
    public GameObject CheckpointPrefab { get { return checkpointPrefab; } }

    [Range(1, 10)]
    [SerializeField]
    private int nbMapToGetCheckPoint = 3;   // TODO
    [SerializeField]
    private int nbMinMapFragmentInSameBiome = 3;

    [Header("Setup")]
    [SerializeField]
    private Transform mapFragmentContainer = default;
    [SerializeField]
    private int mapFragmentSize = 30;
    [SerializeField]
    private string prefabMapFragmentFolder = "";
    [SerializeField]
    private string[] listPrefabMapFragmentSubFolder = new string[1];
    [SerializeField]
    private string prefabJoinMapFragmentFolder = "";
    [Range(0f, 1f)]
    [SerializeField]
    private float chanceToStayInSameBiome = 0.3f;
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
    private int nbMapFragmentGeneratedInCurrentBiome = 0;
    private bool isLastBiomeFolder = false;

    private EnemyGroupGOFactory enemyGroupGOFactory;

    void Start()
    {
        Instance = this;

        enemyGroupGOFactory = GetComponent<EnemyGroupGOFactory>();

        // initialize the folder names to not have doing it all the time while instantiating new map fragment
        listPrefabMapFragmentFolderFullPath = new string[listPrefabMapFragmentSubFolder.Length];
        for (int i = 0; i < listPrefabMapFragmentSubFolder.Length; ++i)
        {
            listPrefabMapFragmentFolderFullPath[i] = prefabMapFragmentFolder + "/" + listPrefabMapFragmentSubFolder[i];
        }
        isLastBiomeFolder = !(listPrefabMapFragmentSubFolder.Length > 1);

        ChargeFirstsMap();
    }

    /**
     * Just need to call it to charge a new map fragment and remove the last one
     */ 
    public void NotifyEndOfMap()
    {
        // remove the last map
        MapFragment firstMapFragment = queueMapFragment.Dequeue();
        firstMapFragment.Destroy();

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

        // if we already have instantiate the minimum fragment map in biome, and if we are not in the last biome folder
        if (nbMapFragmentGeneratedInCurrentBiome >= nbMinMapFragmentInSameBiome -1 && !isLastBiomeFolder && chanceToStayInSameBiome <= Random.Range(0f, 1f))
        {
            nbMapFragmentGeneratedInCurrentBiome = 0;
            Object[] arrayMapFragmentJoinGO = Resources.LoadAll(prefabJoinMapFragmentFolder + '/' + listPrefabMapFragmentSubFolder[currentMapFragmentFolderIndex]);
            // select a join randomly
            mapFragmentToSpawn = (GameObject)arrayMapFragmentJoinGO[Random.Range(0, arrayMapFragmentJoinGO.Length - 1)];

            // change the map fragment biome
            if (listPrefabMapFragmentFolderFullPath.Length - 1 > currentMapFragmentFolderIndex)
            {
                ++currentMapFragmentFolderIndex;
                // charge the following map fragments
                mapFragmentSelection = Resources.LoadAll(listPrefabMapFragmentFolderFullPath[currentMapFragmentFolderIndex], typeof(GameObject));
                if (currentMapFragmentFolderIndex == listPrefabMapFragmentFolderFullPath.Length - 1)
                {
                    isLastBiomeFolder = true;
                }
            }
        }
        else
        {
            mapFragmentToSpawn = (GameObject) mapFragmentSelection[Random.Range(0, mapFragmentSelection.Length - 1)];
            ++nbMapFragmentGeneratedInCurrentBiome;
        }

        SpawnMap(mapFragmentToSpawn);
    }

    private void SpawnFirstMapFragment()
    {
        float distance = -firstMapFragmentSize;
        Vector3 pos = mapFragmentContainer.forward * distance;
        GameObject mapFragmentGO = Instantiate(firstMapFragmentPrefab, pos, mapFragmentContainer.rotation, mapFragmentContainer);
        MapFragment mapFragment = mapFragmentGO.GetComponent<MapFragment>();
        queueMapFragment.Enqueue(mapFragment);
    }

    private void SpawnMap(GameObject mapFragmentToSpawn)
    {
        float distance = nbMapFragmentGenerated * mapFragmentSize;
        Vector3 pos = mapFragmentContainer.forward * distance;
        GameObject mapFragmentGO = Instantiate(mapFragmentToSpawn, pos, mapFragmentContainer.rotation, mapFragmentContainer);
        MapFragment mapFragment = mapFragmentGO.GetComponent<MapFragment>();
        queueMapFragment.Enqueue(mapFragment);

        // if we need to spawna checkpoint
        bool needToSpawnCheckPoint = nbMapFragmentGenerated % nbMapToGetCheckPoint == 0 && nbMapFragmentGenerated != 0;
 
        ++nbMapFragmentGenerated;

        // instantiate enemies, princes...

        // TODO need better than that v1.0
        GameObject[] arrayEnemyGroupGO = new GameObject[3];
        GameObject[] arrayPrinceGO = new GameObject[3];
        for(int i = 0; i < 3; ++i)
        {
            arrayEnemyGroupGO[i] = enemyGroupGOFactory.GetEnemyGroupGO(nbMapFragmentGenerated);
            arrayPrinceGO[i] = princePrefab;
        }

        mapFragment.InitWith(arrayEnemyGroupGO, arrayPrinceGO, needToSpawnCheckPoint);
    }
}
