using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Vector3 Position { get { return pointToDestroyAgent.position; } }

    [Header("Setup")]
    [SerializeField]
    private Transform pointToDestroyAgent = default;
}
