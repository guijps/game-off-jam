using Godot;
using System;

public abstract partial class Enemy : CharacterBody2D
{
	[Export]
	public Label LifeLabel;
	[Export]
	public Weapon weapon;

	public StatsComponent Stats;

	protected NavigationAgent2D _navigationAgent;

	public ProjectileStatsComponent ProjectileStats;
	protected EnemyState CurrentState;
	protected EnemyState NextState;
	protected GameManager gameManager;
	#region FireSpecifications
	protected double lastFireTime = 0;
	protected double FireCooldown {get { return 1.0 / Stats.CurrentDexterity; }}

	[Export]protected  float ProjectileSpeed = 150f;
	[Export]protected  float ProjectileLifetime = 3f;
	#endregion

	[Export] protected int ExperienceOnDeath = 20;

	public override void _Ready()
    { 	
		gameManager = (GameManager) GetNode("/root/main");
		SetStats();
		lastFireTime = Time.GetUnixTimeFromSystem();
		_navigationAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");

	
		LifeLabel.Text = "HP: " + Stats.CurrentHealth;
    }
	public abstract void SetStats();

	public override void _PhysicsProcess(double delta)
    {
		UpdateState();
		if((CurrentState & EnemyState.Dead)	!= 0)
			return;
		if((CurrentState & EnemyState.Attack) != 0)
			FireCommand();
		if((CurrentState & EnemyState.Move) != 0)
			MovementCommand();
		UpdateAnimation();
		MoveAndSlide();
    }

    public virtual void UpdateAnimation()
    {
        return;
    }

    protected abstract void MovementCommand();
	protected virtual void FireCommand()
    {
		var now = Time.GetUnixTimeFromSystem();
		if(lastFireTime + FireCooldown <= now)
        {
            lastFireTime = now;
			weapon.Fire(ProjectileStats);
        }
		
    }

	protected abstract void UpdateState();
	
	public void OnBodyEntered(Node2D body)
	{
		if(body is Projectile){
			var projectile = body as Projectile;
			if(!projectile.IsFriendly)
				return;
			
			//GD.Print("Enemy Hit by Projectile Damage:" + projectile.Damage);
            if (Stats.TakeDamage(projectile.Damage))
            {
				LifeLabel.Text = "HP: " + Stats.CurrentHealth;
                Die();
			
            }
			LifeLabel.Text = "HP: " + Stats.CurrentHealth;
		}
		// Implement damage logic here
	}

	public void Die()
	{
		NextState = EnemyState.Dead;
		gameManager.EnemyDiedEvent?.Invoke(ExperienceOnDeath);
		QueueFree();
	}
	

}