using Godot;
using System;

public partial class EnemyBaseProjectile : Projectile
{
    public EnemyBaseProjectile()
    {
        IsFriendly = false; // Ensure this projectile is marked as enemy
    }

    public override void _Ready()
    {
        base._Ready();
    }

    // You can override HandleHit or other methods if enemy projectiles need special behavior
    protected override void HandleHit(Node target)
    {
        if (!IsPiercing)
            QueueFree(); // Destroy bullet if not piercing
    }
}
