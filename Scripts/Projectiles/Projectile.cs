using Godot;
using System;

public partial class Projectile : Area2D
{	
	[Export] public bool IsFriendly = false;
    [Export] public float Speed = 200f;          // Movement speed
    [Export] public float TimeLife = 10f;       // Seconds before auto-destroy
    [Export] public int Damage = 1;              // Damage dealt to targets
    [Export] public bool IsPiercing = false;     // If true, continues after hitting an enemy
    [Export] public Vector2 Velocity = Vector2.Zero; // Set by shooter before spawn

    public override void _Ready()
    {
        // Auto-destroy after TimeLife seconds
        GetTree().CreateTimer(TimeLife).Timeout += () => QueueFree();

        // Connect collision
        Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
        Connect("area_entered", new Callable(this, nameof(OnBodyEntered)));
    }

    public void SetStats(ProjectileStatsComponent stats, bool isFriendly = false)
    {
        Damage = (int)stats.Damage;
        Speed = stats.Speed;
        TimeLife = stats.Lifetime;
        IsPiercing = stats.IsPiercing;
        IsFriendly = isFriendly;
    }
    
    public void SetVelocity(Vector2 direction)
    {
        Velocity = direction.Normalized();
    }
    public override void _PhysicsProcess(double delta)
    {
        GlobalPosition += Velocity * Speed * (float)delta;
    }

    protected void OnBodyEntered(Node body)
    {
        //GD.Print("Projectile hit: " + body.Name);
        HandleHit(body);
    }

    protected virtual void HandleHit(Node target)
    {
        // Only interact with enemies (adjust the group name if needed)
        if ((IsFriendly && target.IsInGroup("Enemy")) || (!IsFriendly && target.IsInGroup("Players")))
        {
            if (target.HasMethod("ApplyDamage"))
                target.Call("ApplyDamage", Damage);

        }

        if (!IsPiercing)
            QueueFree(); // Destroy bullet if not piercing
    }
}
