
public class StatsAchievement : Achievement
{
    public string GameStatsAttributeCheckEarn { get => gameStatsAttributeCheckEarn; set => gameStatsAttributeCheckEarn = value; }
    protected string gameStatsAttributeCheckEarn;

    protected override int? GetValueFromGameStats(GameStats gameStats)
    {
        return (typeof(GameStats).GetField(gameStatsAttributeCheckEarn).GetValue(gameStats)) as int?;
    }
}
