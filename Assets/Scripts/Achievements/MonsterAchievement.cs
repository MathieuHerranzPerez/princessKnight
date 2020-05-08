using UnityEngine;

public class MonsterAchievement : Achievement
{
    [SerializeField] protected string monsterName = default;

    protected override int? GetValueFromGameStats(GameStats gameStats)
    {       
        return gameStats.dictionaryMonster.ContainsKey(monsterName) ? gameStats.dictionaryMonster[monsterName] : 0;
    }
}
