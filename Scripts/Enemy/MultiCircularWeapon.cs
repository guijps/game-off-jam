using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class MultiCircularWeapon : Weapon
{
	// Called when the node enters the scene tree for the first time.
	List<Node2D> weaponPoints = new List<Node2D>();
	
	[Export] public float Radius = 50f;
	[Export] public int Quantity = 12;
	[Export] public float FireCooldown = 1f; // seconds

	double lastFireTime=0;
	public const float MAX_ANGLE = 360f;
	public override void _Ready()
	{
		// Create Quantity number of Node2D children
		for (int i = 0; i < Quantity; i++)
		{
			var weapon = new Node2D();
            // Add a Sprite2D as a child
            var sprite = new Sprite2D();
            var texture = GD.Load<Texture2D>("res://Graphs/Player/phPlayer.png");
            sprite.Texture = texture;
            weapon.AddChild(sprite);
			weaponPoints.Add(weapon);
		}

		SetCircularPositions();

		foreach (var point in weaponPoints)
		{
			AddChild(point);
		}
	}

    private void SetCircularPositions()
    {
		int count = weaponPoints.Count;
		float angleStep = MAX_ANGLE / count;

		for (int i = 0; i < count; i++)
		{
			float angleInRadians = Mathf.DegToRad(i * angleStep);
			float x = Radius * Mathf.Cos(angleInRadians);
			float y = Radius * Mathf.Sin(angleInRadians);
			weaponPoints[i].Position = new Vector2(x, y);
			//GD.Print("Weapon Point " + i + " Position: " + weaponPoints[i].Position);
		}
		
    }

	public override void Fire(ProjectileStatsComponent projectileStats)
	{
		if (WeaponProjectileScene == null)
		{
			GD.PrintErr("WeaponProjectileScene is not assigned!");
			return;
		}

		foreach (var point in weaponPoints)
		{
			FireFromPoint(point,projectileStats);
		}
	}

    private void FireFromPoint(Node2D point, ProjectileStatsComponent projectileStats)
    {
		var root = GetTree().CurrentScene;
		Vector2 direction = (point.GlobalPosition - GetParent<Node2D>().GlobalPosition).Normalized();
		var bullet =  WeaponProjectileScene.Instantiate<Projectile>();
		float rotation = Mathf.Atan2(direction.Y, direction.X);
		bullet.SetStats(projectileStats);
		bullet.Rotation = rotation;
		bullet.SetVelocity(direction);
		bullet.GlobalPosition = point.GlobalPosition;  // Spawn at player's position
		root.AddChild(bullet);
    }
}
