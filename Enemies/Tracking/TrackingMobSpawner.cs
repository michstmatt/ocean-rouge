using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class TrackingMobSpawner : Node
{
	[Export]
   	public PackedScene MobScene { get; set; }
	
	[Export]
	public int MaxEnemies = 10;
	
	[Export]
	public PathFollow2D MobSpawnLocation;

	IEnumerable<Player> Players;
	
	public void UpdateTimer(bool start)
	{
		var timer = GetNode<Timer>("SpawnTimer");
		if(start)
		{
			timer.Start();
		}
		else
		{
			timer.Stop();
		}
	}

	public void SpawnMob()
	{
		var enemyCount = GetTree().GetNodesInGroup(Constants.MobGroup).Count;
		
		if(enemyCount >= MaxEnemies)
		{
			return;
		}
		
		Players = GetTree().GetNodesInGroup(Constants.PlayerGroup).Select(p => p as Player);
		
		// Create a new instance of the Mob scene.
		TrackingMob mob = MobScene.Instantiate<TrackingMob>();
		MobSpawnLocation.ProgressRatio = GD.Randf();

		// Set the mob's direction perpendicular to the path direction.
		float direction = MobSpawnLocation.Rotation + Mathf.Pi / 2;

		// Set the mob's position to a random location.
		mob.Position = MobSpawnLocation.Position;

		Player nearest = Players.FirstOrDefault();
		float min = float.PositiveInfinity;

		foreach(Player player in Players)
		{
			var distance = mob.Position.DistanceTo(player.Position);
			if (distance < min)
			{
				min = distance;
				nearest = player;
			}
		}

		mob.Target = nearest;

		// Spawn the mob by adding it to the Main scene.
		AddChild(mob);
		
		mob.AddToGroup("mobs");
	}
}
