using Godot;
using System;

public partial class BasePlayerProjectile : Projectile
{
    public override void _Ready()
    {
        base._Ready();
        IsFriendly = true; 
    }

    protected override void HandleHit(Node target)
    {
        // Check if the object is in the Enemy group, and apply damage
        if (target.IsInGroup("Enemy") && target.HasMethod("ApplyDamage"))
            target.Call("ApplyDamage", Damage);

        if (!IsPiercing)
            QueueFree(); // Destroy bullet if not piercing
    }
}
