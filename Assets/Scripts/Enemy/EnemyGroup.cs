using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    public int Difficulty { get { return difficulty; } }

    // the difficulty is set between 1 and 10. If this has to change, check all the references
    [Range(1, 10)]
    [SerializeField]
    private int difficulty = 5;
}