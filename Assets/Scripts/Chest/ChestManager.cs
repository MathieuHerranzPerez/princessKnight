﻿using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    public static ChestManager Instance { get; private set; }

    // TODO need to complete that list with player weapon
    [SerializeField] private List<GameObject> arrayGameObjectInChest = new List<GameObject>(1);

    [SerializeField] private GameObject chestPrefab = default;

    private bool isInitialized = false;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("More than 1 ChestMananger");
        }
        Instance = this;
    }

    public void SpawnChest(Transform whereToSpawn)
    {
        if (!isInitialized)
            InitWithDataBetweenScene();

        GameObject chestGO = (GameObject)Instantiate(chestPrefab, whereToSpawn.position, whereToSpawn.rotation, whereToSpawn);
        Chest chest = chestGO.GetComponent<Chest>();
        int index = Random.Range(0, arrayGameObjectInChest.Count);
        chest.Init(arrayGameObjectInChest[index]);
    }

    private void InitWithDataBetweenScene()
    {
        foreach (int i in DataBetweenScene.listIndexCardSelected)
        {
            arrayGameObjectInChest.Add(MasterDeck.Instance.GetListAllCards()[i].WeaponOnFloor);
        }

        isInitialized = true;
    }
}
