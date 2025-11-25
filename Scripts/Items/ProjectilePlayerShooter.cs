using Godot;
using System;

public partial class ProjectilePlayerShooter : TargetedWeapon
{
	public ProjectileStatsComponent ProjectileStats;
	private double lastFireTime = 0;
	

	public override void _Ready()
	{
		lastFireTime =Time.GetUnixTimeFromSystem();
		
		var players = GetTree().GetNodesInGroup("Players");
		if(players.Count>0){
			Target = players[0] as Node2D;
		}
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		AimUpdate();
	}

	private void AimUpdate()
	{
		Vector2 position = GetParent<Node2D>().GlobalPosition;
		Vector2 direction = (GetTargetPosition()- position).Normalized();
		GlobalPosition = position +1*direction;
	}

	protected override Vector2 GetTargetPosition()
    {
		return Target==null ?  GetGlobalMousePosition() : Target.GlobalPosition;
    }
}
