using Godot;

public partial class MobSpawner : Node
{
	[Export]
   	public PackedScene MobScene { get; set; }
	public void SpawnMob(PathFollow2D mobSpawnLocation)
	{
		// Create a new instance of the Mob scene.
		Mob mob = MobScene.Instantiate<Mob>();
		mobSpawnLocation.ProgressRatio = GD.Randf();

		// Set the mob's direction perpendicular to the path direction.
		float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

		// Set the mob's position to a random location.
		mob.Position = mobSpawnLocation.Position;

		// Add some randomness to the direction.
		direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
		mob.Rotation = direction;

		// Choose the velocity.
		var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
		mob.LinearVelocity = velocity.Rotated(direction);

		// Spawn the mob by adding it to the Main scene.
		AddChild(mob);
		
		mob.AddToGroup("mobs");
	}
}
