using Godot;
using System;

public partial class BaseMoveSplashEnemy : Enemy
{
	private CharacterBody2D Target;
	private Vector2 targetPosition;

	private float DistanceTreshold = 1f;
	double attackPeriod = 5.0;
	double walkPeriod = 3.0;
	double startedWalkTime = 0.0;
	double attackStartTime = 0.0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{		
		base._Ready();
		Target = GetTree().GetNodesInGroup("Players")[0] as CharacterBody2D ?? null;
		CurrentState = EnemyState.Idle;
		SetMoveNextState();
	}

	public override void SetStats()
    {
		Stats = new StatsComponent(20,100f,2,0f,1f);
		ProjectileStats = new ProjectileStatsComponent(Stats.CurrentAttack,50f,5f);
    }
	

    private void SetAttackNextState()
    {
        attackStartTime = Time.GetUnixTimeFromSystem();
        NextState = EnemyState.Attack;
    }

    protected override void MovementCommand()
    {
		_navigationAgent.TargetPosition = targetPosition;
		Velocity = GlobalPosition.DirectionTo(_navigationAgent.GetNextPathPosition()) * Stats.CurrentSpeed;
    }

    protected override void UpdateState()
    {
		CurrentState = NextState;
		switch (CurrentState)
        {
            case EnemyState.Attack:
				UpdateAttackState();
				break;
			case EnemyState.Move:
				UpdateMovementState();
				break;
			default:
				Velocity = Vector2.Zero;
				break;
        }
    }

    private void UpdateMovementState()
    {
		double now = Time.GetUnixTimeFromSystem();
        if(GlobalPosition.DistanceTo(targetPosition) > DistanceTreshold && now < startedWalkTime + walkPeriod)
			return;
        Velocity = Vector2.Zero;
		SetAttackNextState();
			
	}

	private void UpdateAttackState()
	{
		double now = Time.GetUnixTimeFromSystem();
        Velocity = Vector2.Zero;
		if (now >= attackStartTime + attackPeriod)
        {
			SetMoveNextState();
        }
    }
	
	protected override void FireCommand()
    {
		var now = Time.GetUnixTimeFromSystem();
		if(lastFireTime + FireCooldown <= now)
        {
            lastFireTime = now;
			weapon.Fire(ProjectileStats);
        }
		
    }


    private void SetMoveNextState()
    {
		startedWalkTime = Time.GetUnixTimeFromSystem();
        targetPosition = new Vector2(Target.GlobalPosition.X, Target.GlobalPosition.Y);
		NextState = EnemyState.Move;
    }
}
