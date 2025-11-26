using Godot;
using System;

public partial class GameManager : Node
{

	public Action<int> EnemyDiedEvent;
	public Action<string> MissionUpdateEvent;
	public Action<UpdateStatus> PlayerStatsUpdateEvent;
	public Action<StatsComponent> GuiStatusUpdateEvent;
	public Action FinishLastWave;
	public Action<int> StartWaveNoticeEvent;
	public Action LevelUp;
	public Action OnPlayerDied;
	
	[Export] public  Player PlayerInstance;
	[Export] public  MobSpawn MobSpawnerInstance;
	[Export] public  Node2D MapInstance;


    public Action<string> HealthUpdateEvent { get; internal set; }
    public Action<string> ExperienceUpdateEvent { get; internal set; }


}