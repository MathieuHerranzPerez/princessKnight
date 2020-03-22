using System.Collections.Generic;
using UnityEngine;

public class EnemyGroupGOFactory : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField]
    private GameObject[] arrayEnemyGroupGO = new GameObject[1];
    [SerializeField]
    private GameObject[] arrayBossGO = new GameObject[0];

    // ---- INTERN ----
    private Dictionary<int, List<GameObject>> dictionaryEnemyGroupGO;    // <difficulty, enemyGroupGO[]>
    private Dictionary<int, List<GameObject>> dictionaryBossGO;

    private int enemyGroupMaxDifficulty = 0;
    private int bossMaxDifficulty = 0;

    void Awake()
    {
        // create the dictionary
        dictionaryEnemyGroupGO = new Dictionary<int, List<GameObject>>();
        dictionaryBossGO = new Dictionary<int, List<GameObject>>();
        foreach (GameObject enemyGroupGO in arrayEnemyGroupGO)
        {
            EnemyGroup enemyGroup = enemyGroupGO.GetComponent<EnemyGroup>();

            List<GameObject> tmpList;
            if (dictionaryEnemyGroupGO.ContainsKey(enemyGroup.Difficulty))
            {
                // replace the current list by the new one with old and current value
                tmpList = dictionaryEnemyGroupGO[enemyGroup.Difficulty];
                tmpList.Add(enemyGroupGO);

                dictionaryEnemyGroupGO[enemyGroup.Difficulty] = tmpList;
            }
            else
            {
                // create it
                tmpList = new List<GameObject>();
                tmpList.Add(enemyGroupGO);

                dictionaryEnemyGroupGO.Add(enemyGroup.Difficulty, tmpList);
            }

            if(enemyGroup.Difficulty > enemyGroupMaxDifficulty)
            {
                enemyGroupMaxDifficulty = enemyGroup.Difficulty;
            }
        }

        foreach (GameObject bossGO in arrayBossGO)
        {
            Boss boss = bossGO.GetComponent<Boss>();

            List<GameObject> tmpList;
            if (dictionaryBossGO.ContainsKey(boss.Difficulty))
            {
                // replace the current list by the new one with old and current value
                tmpList = dictionaryBossGO[boss.Difficulty];
                tmpList.Add(bossGO);

                dictionaryBossGO[boss.Difficulty] = tmpList;
            }
            else
            {
                // create it
                tmpList = new List<GameObject>();
                tmpList.Add(bossGO);

                dictionaryBossGO.Add(boss.Difficulty, tmpList);
            }

            if (boss.Difficulty > bossMaxDifficulty)
            {
                bossMaxDifficulty = boss.Difficulty;
            }
        }
    }

    /**
     * return an EnemyGroupGO that fits the difficulty
     * null if no EnemyGroupGO is found
     */ 
    public GameObject GetEnemyGroupGO(int difficulty)
    {
        GameObject resGO = null;
        if(dictionaryEnemyGroupGO.ContainsKey(difficulty))
        {
            resGO = dictionaryEnemyGroupGO[difficulty][Random.Range(0, dictionaryEnemyGroupGO[difficulty].Count)];
        }
        else
        {
            // find the nearest difficulty
            int difficultyUp = difficulty;
            int difficultyDown = difficulty;

            while (difficultyUp <= 10 || difficultyDown >= 1)
            {
                ++difficultyUp;
                --difficultyDown;

                if(dictionaryEnemyGroupGO.ContainsKey(difficultyDown))
                {
                    resGO = dictionaryEnemyGroupGO[difficultyDown][Random.Range(0, dictionaryEnemyGroupGO[difficultyDown].Count)];
                }
                else if(dictionaryEnemyGroupGO.ContainsKey(difficultyUp))
                {
                    resGO = dictionaryEnemyGroupGO[difficultyUp][Random.Range(0, dictionaryEnemyGroupGO[difficultyUp].Count)];
                }
            }
        }

        return resGO;
    }

    public GameObject GetBossGO(int difficulty)
    {
        GameObject resGO = null;
        if (dictionaryBossGO.ContainsKey(difficulty))
        {
            resGO = dictionaryBossGO[difficulty][Random.Range(0, dictionaryBossGO[difficulty].Count)];
        }
        else
        {
            // find the nearest difficulty
            int difficultyUp = difficulty;
            int difficultyDown = difficulty;

            while (difficultyUp <= 10 || difficultyDown >= 1)
            {
                ++difficultyUp;
                --difficultyDown;

                if (dictionaryBossGO.ContainsKey(difficultyDown))
                {
                    resGO = dictionaryBossGO[difficultyDown][Random.Range(0, dictionaryBossGO[difficultyDown].Count)];
                }
                else if (dictionaryBossGO.ContainsKey(difficultyUp))
                {
                    resGO = dictionaryBossGO[difficultyUp][Random.Range(0, dictionaryBossGO[difficultyUp].Count)];
                }
            }
        }

        // TODO
        // if(diffilty > enemyGroupMaxDifficulty + 2)
        // need to multiply Enemy STATS

        return resGO;
    }
}
