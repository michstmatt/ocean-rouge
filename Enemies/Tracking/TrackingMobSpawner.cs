using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Godot;

public partial class TrackingMobSpawner : Node
{
	[Export]
   	public PackedScene MobScene { get; set; }
	
	[Export]
	public int MaxEnemies = 10;

	[Export]
	float distanceFrom = 500f;

	float velocity = 80f;
	float rate;

	[Export]
	public float MinSpeed = 1.0f;

	int time = 0;
	
	IEnumerable<Player> Players;
	Timer SpawnTimer;

	EnemyStateManager enemyStateManager = new EnemyStateManager();
	void Init()
	{
		velocity = 100f;
		SpawnTimer.WaitTime = 1;
		MaxEnemies = 20;
	}
	
	public void UpdateTimer(bool start)
	{
		SpawnTimer = GetNode<Timer>("SpawnTimer");
		if(start)
		{
			Init();
			SpawnTimer.Start();
		}
		else
		{
			SpawnTimer.Stop();
		}
	}

	public void UpdateFrequency(int time)
	{
		if (time % 10 == 0)
		{
			velocity += 20;
			MaxEnemies += 5;
			if (SpawnTimer.WaitTime > MinSpeed)
			{
				SpawnTimer.WaitTime -= .1;
			}
		}
	}

	public void SpawnMob()
	{
		
		UpdateFrequency(time++);

		var enemyCount = GetTree().GetNodesInGroup(Constants.MobGroup).Count;
		
		if(enemyCount >= MaxEnemies)
		{
			return;
		}
		
		Players = GetTree().GetNodesInGroup(Constants.PlayerGroup).Select(p => p as Player);

		var selectedPlayer = Players.ElementAt((int)(GD.Randi() % Players.Count()));
		
		// Create a new instance of the Mob scene.
		TrackingMob mob = MobScene.Instantiate<TrackingMob>();

		float angle = GD.Randf() * 2 * Mathf.Pi;

		var pos = Godot.Vector2.FromAngle(angle) * distanceFrom;

		mob.Position = selectedPlayer.Position + pos;
		mob.Target = selectedPlayer;

		EnemyType random = enemyStateManager.GetRandomEnemy();

		mob.EnemyType = random;

		// Spawn the mob by adding it to the Main scene.
		AddChild(mob);
		
		mob.AddToGroup("mobs");
	}
}
