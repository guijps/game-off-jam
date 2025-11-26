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
        if (!IsPiercing)
            QueueFree(); // Destroy bullet if not piercing
    }
}
