using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public float couldDown = 3f;

    private float time = -1f;

    private void Update()
    {
        if (time <= 0f)
        {
            Spawn();
            time = couldDown;
        }
        else
        {
            time -= Time.deltaTime;
        }
    }

    private void Spawn()
    {
        Instantiate(prefabToSpawn, transform.position, transform.rotation);
    }
}
