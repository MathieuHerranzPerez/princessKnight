
public interface Damageable
{
    void TakeDamage(int amount, DamageSource source);
}

public enum DamageSource
{
    PLAYER,
    ENEMY
}

