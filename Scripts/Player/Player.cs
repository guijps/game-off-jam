using Godot;
using System;
using System.Diagnostics.CodeAnalysis;

public partial class Player :CharacterBody2D
{
	[Export] public float MoveSpeed = 300f;
	[Export] public PlayerGun weapon = null;
	[Export] public Label LifeLabel;
	[Export] public int Experience = 0;
	[Export] public int ExperienceToNextLevel = 100;
	[Export] public int Level = 1;

 	[Export] public float MaxHealth = 100f;
	[Export] public float Dexterity = 1;
	[Export] public float Vitality = 1;
	[Export] public float Attack = 1;	
	public float FireCooldown
    {
        get
        {
            return 1.0f / Stats.CurrentDexterity;
        }
    }
	public StatsComponent Stats;
	public ProjectileStatsComponent ProjectileStats;
    private double lastTimeFired;
	[Export]private GameManager gameManager;
    public override void _Ready()
	{
		gameManager.EnemyDiedEvent += AddExperience;
		gameManager.PlayerStatsUpdateEvent += LevelUpStats;
		Stats = new StatsComponent(MaxHealth,MoveSpeed,Dexterity,Vitality,Attack);
		ProjectileStats = new ProjectileStatsComponent(Stats.CurrentAttack,200f,3,false);
		weapon.Stats = ProjectileStats;
		lastTimeFired = Time.GetUnixTimeFromSystem();
		UpdateHealth();
	}
	public override void _PhysicsProcess(double delta)
	{
		MovementCommand();
		
		float fire = Input.GetActionStrength("shoot");
		if(fire > 0 )
			FireCommand();
		MoveAndSlide();
	}

    private void FireCommand()
    {
		double now = Time.GetUnixTimeFromSystem();
		if(lastTimeFired + FireCooldown > now)
			return;
		lastTimeFired = now;
     	weapon.Fire(ProjectileStats);
    }

    private void MovementCommand()
	{
		float hor = Input.GetActionStrength("right")- Input.GetActionStrength("left");
		float ver = Input.GetActionStrength("down") - Input.GetActionStrength("up"); 
		Vector2 directionMove = new Vector2(hor,ver);
		Velocity = directionMove * Stats.CurrentSpeed;
	}
	
	public void OnBodyEntered(Node2D body)
	{
		if(body is Projectile)
		{
			var projectile = body as Projectile;
			if(projectile.IsFriendly)
				return;
			UpdateHealth();
			if (Stats.TakeDamage(projectile.Damage))
				Die();

		}
	}

    private void Die()
    {
		gameManager.OnPlayerDied?.Invoke();
        QueueFree();
		
    }

    public void AddExperience(int amount)
	{
		Experience += amount;
		while(Experience >= ExperienceToNextLevel)
		{
			Experience -= ExperienceToNextLevel;
			LevelUp();
		}
		GD.Print("Player Experience: " + Experience + "/" + ExperienceToNextLevel);
	}

//This logis should be at StatsComponent
	public void LevelUp()
	{
		gameManager.LevelUp?.Invoke();
	}
	public void UpdateHealth()
    {
		gameManager.HealthUpdateEvent?.Invoke("HP: " + Stats.CurrentHealth + "/" + Stats.MaxHealth);
    }

	public void LevelUpStats(UpdateStatus status){
		Stats.LevelUp(status);
		ProjectileStats.LevelUp(Stats);
		gameManager.GuiStatusUpdateEvent?.Invoke(Stats);
	}
}
