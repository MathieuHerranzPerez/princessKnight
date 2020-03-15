public class EnemyDieEvent : IEvent
{
    private Enemy enemy;

    public EnemyDieEvent(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public object GetData()
    {
        return this.enemy;
    }

    public EventName GetName()
    {
        return EventName.EnemyDeath;
    }
}
