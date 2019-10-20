using System;
using UnityEngine;

public class MapFragment : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField]
    private SpawnChunck[] arrayEnemyAndPrinceSpawnPoint = new SpawnChunck[1];

    public void Destroy()
    {
        // TODO remove all content


        Destroy(transform.gameObject);
    }
}

[System.Serializable]
public class SpawnChunck
{
    public Transform enemySpawnPoint;
    public Transform princeSpawnPoint;
}