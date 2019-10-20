using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    public int Difficulty { get { return difficulty; } }

    [Range(1, 10)]
    [SerializeField]
    private int difficulty = 5;
}
