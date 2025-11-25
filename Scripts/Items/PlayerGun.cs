using Godot;
using System;

public partial class PlayerGun : TargetedWeapon
{

	
	protected override Vector2 GetTargetPosition()
	{
		return GetGlobalMousePosition();
	}	 
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		AimUpdate();
	}

	private void AimUpdate()
	{
		Vector2 position = GetParent<Node2D>().GlobalPosition;
		Vector2 direction = (GetTargetPosition() - position).Normalized();
		GlobalPosition = position +30*direction;
	}

		
	
}
