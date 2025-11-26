using Godot;
using System;

public partial class BaseFollowShootingEnemy : Enemy
{	
	private CharacterBody2D Target;
	
	public override void _Ready()
    { 
		base._Ready();
		CurrentState = EnemyState.Move | EnemyState.Attack;
		Target = GetTree().GetNodesInGroup("Players")[0] as CharacterBody2D ?? null;
		//GD.Print("Enemy Target: " + Target.Name);
    }
	public override void SetStats()
    {
		Stats = new StatsComponent(20,100f,2,0f,5f);
		ProjectileStats = new ProjectileStatsComponent(Stats.CurrentAttack,150f,Stats.CurrentAttack);
    }
	
    protected override void UpdateState()
    {
        return;
    }

    protected override void MovementCommand()
	{
		if(Target == null )
			return;
		
		_navigationAgent.TargetPosition = Target.GlobalPosition;
		Velocity = GlobalPosition.DirectionTo(_navigationAgent.GetNextPathPosition()) * Stats.CurrentSpeed;
	}

}
