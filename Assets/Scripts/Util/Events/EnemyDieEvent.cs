public class EnemyDieEvent : IEvent
{
    private string enemyName;

    public EnemyDieEvent(string enemyName)
    {
        this.enemyName = enemyName;
    }

    public object GetData()
    {
        return enemyName;
    }

    public string GetName()
    {
        return "enemyDie";
    }
}
