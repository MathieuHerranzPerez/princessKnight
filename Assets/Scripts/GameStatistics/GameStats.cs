
using System.Collections.Generic;

[System.Serializable] public class DictionaryMonster : Dictionary<string, int> { }

[System.Serializable]
public class GameStats
{
    public int damageDealt = 0;
    public int nbEnemyKilled = 0;
    public int nbBossKilled = 0;
    public DictionaryMonster dictionaryMonster = new DictionaryMonster();
}
