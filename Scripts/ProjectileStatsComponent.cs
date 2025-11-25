using System;

public class ProjectileStatsComponent
{
    public float Damage { get; set; }
    public float Speed { get; set; }
    public float Lifetime { get; set; }
    public bool IsPiercing { get; set; }

    public ProjectileStatsComponent(float damage, float speed, float lifetime, bool isPiercing = false)
    {
        Damage = damage;
        Speed = speed;
        Lifetime = lifetime;
        IsPiercing = isPiercing;
    }

    public void LevelUp(StatsComponent stats)
    {
        Damage = stats.CurrentAttack;
        Speed = stats.CurrentSpeed;
    }
}
