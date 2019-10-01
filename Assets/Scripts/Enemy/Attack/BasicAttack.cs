

public class BasicAttack : EnemyAttack
{
    public override void Cast()
    {
        target.TakeDamage(damageOnHit);
    }
}
