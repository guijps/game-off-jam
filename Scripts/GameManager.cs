using Godot;
using System;

public partial class GameManager : Node
{

	public Action<int> EnemyDiedEvent;
	public Action<string> MissionUpdateEvent;
	public Action<UpdateStatus> PlayerStatsUpdateEvent;
	public Action<StatsComponent> GuiStatusUpdateEvent;
	public Action<int> StartWaveNoticeEvent;
	public Action LevelUp;
	public Action OnPlayerDied;
	
	[Export] public  Player PlayerInstance;
	[Export] public  MobSpawn MobSpawnerInstance;

    public Action<string> HealthUpdateEvent { get; internal set; }
    public Action<string> ExperienceUpdateEvent { get; internal set; }

    public override void _Ready()
    {
		OnPlayerDied += HandlePlayerDied;
    }

	public void HandlePlayerDied()
	{
		GD.Print("Game Over!");
		// Implement game over logic 
		// here (e.g., show game over screen, restart level, etc.)
	}

	public void StartWave()
    {
        
    }
}