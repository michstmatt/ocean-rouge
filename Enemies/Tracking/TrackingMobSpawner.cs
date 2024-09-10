using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Godot;

public partial class TrackingMobSpawner : Node
{
   	public Dictionary<EnemyType,PackedScene> MobScenes { get; set; }
	
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


	public override void _Ready()
	{
		MobScenes = new Dictionary<EnemyType,PackedScene>();
		foreach(EnemyType type in EnemyFactory.EnemyTypes)
		{
			var scene = EnemyFactory.GetEnemyScene(type);
			MobScenes.Add(type, (PackedScene)ResourceLoader.Load(scene));
		}
	}

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

		EnemyType enemyType  = EnemyFactory.GetRandomEnemyType();
		TrackingMob mob = MobScenes[enemyType].Instantiate<TrackingMob>();

		float angle = GD.Randf() * 2 * Mathf.Pi;

		var pos = Godot.Vector2.FromAngle(angle) * distanceFrom;

		mob.Position = selectedPlayer.Position + pos;
		mob.Target = selectedPlayer;

		// Spawn the mob by adding it to the Main scene.
		AddChild(mob);
		
		mob.AddToGroup(Constants.MobGroup);
		
	}
}
