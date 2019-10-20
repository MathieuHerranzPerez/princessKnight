using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

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

    // ---- INTERN ----
    private string[] listPrefabMapFragmentFolderFullPath;
    private Queue<MapFragment> queueMapFragment = new Queue<MapFragment>();
    private int currentMapFragmentFolderIndex = 0;
    private Object[] mapFragmentSelection;                              // store the mapFragment Object loaded from Ressources folder 
    private int nbMapFragmentGenerated = 0;
    private int nbMapFragmentGeneratedInCurrentBiome = 0;
    private bool isLastBiomeFolder = false;

    void Start()
    {
        Instance = this;
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
        GameObject mapFragmentGO = Instantiate(firstMapFragmentPrefab, Vector3.zero, mapFragmentContainer.rotation, mapFragmentContainer);
        Vector3 pos = new Vector3(0f, 0f, -firstMapFragmentSize);
        mapFragmentGO.transform.localPosition = pos;
        mapFragmentGO.transform.localRotation = Quaternion.identity;
        MapFragment mapFragment = mapFragmentGO.GetComponent<MapFragment>();
        queueMapFragment.Enqueue(mapFragment);
    }

    private void SpawnMap(GameObject mapFragmentToSpawn)
    {
        GameObject mapFragmentGO = Instantiate(mapFragmentToSpawn, Vector3.zero, mapFragmentContainer.rotation, mapFragmentContainer);
        Vector3 pos = new Vector3(0f, 0f, nbMapFragmentGenerated * mapFragmentSize);
        mapFragmentGO.transform.localPosition = pos;
        mapFragmentGO.transform.localRotation = Quaternion.identity;
        MapFragment mapFragment = mapFragmentGO.GetComponent<MapFragment>();
        queueMapFragment.Enqueue(mapFragment);

        ++nbMapFragmentGenerated;

        // deal with navMesh

        // todo instantiate enemies, princes...
    }
}
