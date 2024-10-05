using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Serialization;
using Godot;

public partial class TrackingMobSpawner : Node
{
	public Dictionary<EnemyType, PackedScene> MobScenes { get; set; }

	public LevelState LevelData;

	[Export]
	float distanceFrom = 500f;

	float velocity = 80f;

	public int DeadEnemyCount = 0;

	[Export]
	public float MinSpeed = 1.0f;

	int time = 0;

	IEnumerable<Player> Players;
	Timer SpawnTimer;


	public override void _Ready()
	{
		base._Ready();
		MobScenes = new Dictionary<EnemyType, PackedScene>();
		EnemyFactory.Reinit();
		foreach (EnemyType type in EnemyFactory.EnemyTypes)
		{
			var scene = EnemyFactory.GetEnemyMetadata(type).Scene;
			MobScenes.Add(type, (PackedScene)ResourceLoader.Load(scene));
		}
		SpawnTimer = GetNode<Timer>("SpawnTimer");

		SignalManager.Instance.GameOver += Reset;
		SignalManager.Instance.PauseGame += (isPaused) => UpdateTimer(!isPaused);
		SignalManager.Instance.NextLevel += NextLevel;
		SignalManager.Instance.EnemyDied += (a,b) => {
			DeadEnemyCount++;
		};
	}

	void Reset(bool gameOver)
	{
		if (gameOver)
		{
			GetTree().CallGroup(Constants.MobGroup, "QueueFree");
			Init();
		}
		UpdateTimer(!gameOver);
	}

	void Init()
	{
		time = 0;
		velocity = 100f;
		SpawnTimer.WaitTime = 1;
		DeadEnemyCount = 0;
		EnemyFactory.Reinit();
	}

	void NextLevel(int level)
	{
		LevelData = LevelManager.Instance.GetCurrentLevel();
		SpawnTimer.WaitTime = LevelData.EnemySpawnRate;
		DeadEnemyCount = 0;
		EnemyFactory.OnNextLevel(level);
	}

	public void UpdateTimer(bool start)
	{
		if (start)
		{
			SpawnTimer.Start();
			Init();
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
			velocity += 10;
			if (SpawnTimer.WaitTime > MinSpeed)
			{
				SpawnTimer.WaitTime -= .1;
			}
		}
	}

	public void SpawnMob()
	{
		if (DeadEnemyCount >= LevelData.NumEnemies)
		{
			return;
		}

		UpdateFrequency(time++);

		var enemyCount = GetTree().GetNodesInGroup(Constants.MobGroup).Count;

		if (enemyCount >= LevelData.NumConcurrentEnemies)
		{
			return;
		}

		Players = GetTree().GetNodesInGroup(Constants.PlayerGroup).Select(p => p as Player);

		var selectedPlayer = Players.ElementAt((int)(GD.Randi() % Players.Count()));

		EnemyType enemyType = EnemyFactory.GetRandomEnemyType();
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
