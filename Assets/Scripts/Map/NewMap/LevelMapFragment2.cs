using UnityEngine;

public class LevelMapFragment2 : LevelMapFragment
{
    [SerializeField] private MapFragmentDoor[] arrayDoorPointGO = new MapFragmentDoor[1];

    void Start()
    {
        // activate a door
        System.Random rand = new System.Random();
        int index = rand.Next(arrayDoorPointGO.Length);
        arrayDoorPointGO[index].Activate();
    }
}
