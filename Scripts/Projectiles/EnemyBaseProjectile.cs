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
        //Here check if Object is in player group, if it is, apply damage 
        // Only interact with enemies (adjust the group name if needed)
        if (target.IsInGroup("Players") && target.HasMethod("ApplyDamage"))
                target.Call("ApplyDamage", Damage);
                
        if (!IsPiercing)
            QueueFree(); // Destroy bullet if not piercing
    }
}
