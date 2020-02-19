using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager2 : MonoBehaviour
{
    public static MapManager2 Instance { get; private set; }

    [Range(0f, 1f)]
    [SerializeField] private float chanceToSpawnChest = 0.2f;
    [Range(1, 3)]
    [SerializeField] private int nbMaxChestPerFragment = 2;

    [Header("Setup")]
    [SerializeField] private int nbMapFragmentToCheckPoint = 4;
    [SerializeField] private Transform mapFragmentContainer = default;
    [SerializeField]
    private string prefabMapFragmentFolder = "";
    [SerializeField]
    private string[] listPrefabMapFragmentSubFolder = new string[1];

    [Header("Checkpoint map fragment")]
    [SerializeField]
    private GameObject[] listPrefabCheckpointMapFragment = default;

    [Header("Prince")]
    [SerializeField]
    private GameObject princePrefab = default;

    // ---- INTERN ----
    private string[] listPrefabMapFragmentFolderFullPath;
    private MapFragment lastMapFragment;
    private int currentMapFragmentFolderIndex = 0;
    private Object[] mapFragmentSelection;                              // store the mapFragment Object loaded from Ressources folder 
    private int nbMapFragmentGenerated = 0;
    private int nbCheckpointMapFragment = 0;
    private int nbEndOfMap = 0;
    private bool needToSpawnCheckPoint = false;

    private EnemyGroupGOFactory enemyGroupGOFactory;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        enemyGroupGOFactory = GetComponent<EnemyGroupGOFactory>();

        // initialize the folder names to not have doing it all the time while instantiating new map fragments
        listPrefabMapFragmentFolderFullPath = new string[listPrefabMapFragmentSubFolder.Length];
        for (int i = 0; i < listPrefabMapFragmentSubFolder.Length; ++i)
        {
            listPrefabMapFragmentFolderFullPath[i] = prefabMapFragmentFolder + "/" + listPrefabMapFragmentSubFolder[i];
        }

        ChargeNewMap();
    }

    /**
    * Just need to call it to charge a new map fragment and remove the last one
    */
    public void NotifyEndOfMap()
    {
        ++nbEndOfMap;

        // destroy the previous one
        lastMapFragment.Destroy();

        // instantiate a map
        ChargeNewMap();
    }


    private void ChargeNewMap()
    {
        GameObject mapFragmentToSpawn;

        if (!needToSpawnCheckPoint)
        {
            // charge the following map fragments
            mapFragmentSelection = Resources.LoadAll(listPrefabMapFragmentFolderFullPath[currentMapFragmentFolderIndex], typeof(GameObject));
            mapFragmentToSpawn = (GameObject)mapFragmentSelection[Random.Range(0, mapFragmentSelection.Length - 1)];
            


            ++currentMapFragmentFolderIndex;
            if (currentMapFragmentFolderIndex == listPrefabMapFragmentFolderFullPath.Length)
            {
                currentMapFragmentFolderIndex = 0;
            }

            ++nbMapFragmentGenerated;
        }
        else
        {
            mapFragmentToSpawn = listPrefabCheckpointMapFragment[Random.Range(0, listPrefabCheckpointMapFragment.Length - 1)];
            ++nbCheckpointMapFragment;
        }

        SpawnMap(mapFragmentToSpawn);
    }

    private void SpawnMap(GameObject mapFragmentToSpawn)
    {
        Debug.Log("Spawn map");
        GameObject mapFragmentGO = Instantiate(mapFragmentToSpawn, transform.position, transform.rotation, mapFragmentContainer);
        LevelMapFragment levelMapFragment = mapFragmentGO.GetComponent<LevelMapFragment>();
        lastMapFragment = levelMapFragment;

        if (needToSpawnCheckPoint)
        {

            // instantiate enemies, princes...

            // TODO need better than that v1.0
            GameObject[] arrayEnemyGroupGO = new GameObject[7];
            GameObject[] arrayPrinceGO = new GameObject[5];
            for (int i = 0; i < 7; ++i)
            {
                arrayEnemyGroupGO[i] = enemyGroupGOFactory.GetEnemyGroupGO(nbMapFragmentGenerated);
            }
            for (int i = 0; i < 5; ++i)
            {
                arrayPrinceGO[i] = princePrefab;
            }


            int nbChestToSpawn = 0;
            for (int i = 0; i < nbMaxChestPerFragment; ++i)
            {
                float random = Random.Range(0f, 1f);
                if (random < chanceToSpawnChest)
                {
                    ++nbChestToSpawn;
                }
            }

            levelMapFragment.InitWith(arrayEnemyGroupGO, arrayPrinceGO, nbChestToSpawn);
        }

        // if we need to spawn a checkpoint as next map
        needToSpawnCheckPoint = ((nbMapFragmentGenerated % nbMapFragmentToCheckPoint) == 0 && nbMapFragmentGenerated > 0);
    }
}
