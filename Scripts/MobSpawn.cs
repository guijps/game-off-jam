using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class MobSpawn : Node2D
{
	GameManager gameManager;

	int wave;
	Dictionary<EnemyType,int> spawnedMobsCount = new Dictionary<EnemyType,int>();
	[Export] public PackedScene CryBabyScene;
	[Export] public PackedScene RichGuyScene;
	[Export] public PackedScene GhostScene;
	[Export] public int LastWave = 10;
	public int EnemiesToKill
	{
		get;set;
	}
	private readonly object _lock = new object();
	public override void _Ready()
	{
		wave =1;
		gameManager = GetNode<GameManager>("/root/main");
		CryBabyScene = GD.Load<PackedScene>("res://Scenes/Enemy/enemy_cry_baby.tscn");
		RichGuyScene = GD.Load<PackedScene>("res://Scenes/Enemy/rich_guy.tscn");
		gameManager.StartWaveNoticeEvent?.Invoke(wave);
		gameManager.EnemyDiedEvent += async (type) => await UpdateMissionOnDeath(type);
		SpawnWave();
	}
	public void SpawnWave()

	{
		spawnedMobsCount[EnemyType.CryBaby] = wave ;
		spawnedMobsCount[EnemyType.RichGuy] = wave* 2;
		//spawnedMobsCount[EnemyType.Ghost] = wave / 2;
		EnemiesToKill= wave * 3;
		foreach(var mobEntry in spawnedMobsCount)
		{
			for(int i=0;i< mobEntry.Value;i++)
			{
				SpawnMob(mobEntry.Key);
			}
		}
		wave++;
	}
	public void SpawnMob(EnemyType type)
	{
		PackedScene sceneToSpawn = null;
		switch(type)
		{
			case EnemyType.CryBaby:
				sceneToSpawn = CryBabyScene;
				break;
			case EnemyType.RichGuy:
				sceneToSpawn = RichGuyScene;
				break;
			case EnemyType.Ghost:
				sceneToSpawn = GhostScene;
				break;
		}
		if(sceneToSpawn == null)
		{
			GD.PrintErr("No scene found for enemy type: " + type);
			return;
		}
		var mobInstance = sceneToSpawn.Instantiate<Enemy>();
		double x = GD.RandRange(20, 580);
		double y = GD.RandRange(20,280);
		Vector2 spawnPosition = new Vector2(
			(float)x,
			(float)y
		);
		mobInstance.Position = spawnPosition;
		//GD.Print("Spawned " + type + " at " + spawnPosition);
		AddChild(mobInstance);
	}
	public async Task UpdateMissionOnDeath(int type)
	{
		lock (_lock)
		{
			if(EnemiesToKill <= 0)
			return;
			EnemiesToKill--;
		}
		//GD.Print("Enemies left to kill: " + EnemiesToKill);
		gameManager.MissionUpdateEvent?.Invoke("Enemies left to kill: " + EnemiesToKill);
		if(EnemiesToKill <= 0)
		{
			await FinishWave();
		}	
	}
	private async Task FinishWave()
	{
		if(wave==LastWave +1)
		{
			//GD.Print("All waves completed! You win!");
			gameManager.FinishLastWave?.Invoke();

		}
		//GD.Print("Wave cleared! Spawning next wave...");
		gameManager.StartWaveNoticeEvent?.Invoke(wave);
		await ToSignal(GetTree().CreateTimer(3.0f), "timeout");
		SpawnWave();
	}
		
	
}
