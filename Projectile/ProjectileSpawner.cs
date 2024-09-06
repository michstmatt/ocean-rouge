using Godot;
using System;

public partial class ProjectileSpawner : Node
{
	[Export]
   	public PackedScene ProjectileScene { get; set; }

	public void UpdateTimer(bool start)
	{
		var timer = GetNode<Timer>("AutoFireTimer");
		if(start)
		{
			timer.Start();
		}
		else
		{
			timer.Stop();
		}
	}
	
	private void FireProjectile()
	{
		var parent = GetParent<Player>();
		Projectile bullet = ProjectileScene.Instantiate<Projectile>();
		
		bullet.Position = parent.Position;
		bullet.Velocity = parent.LastMove;
		bullet.Rotation = parent.LastMove.Angle() + 0.5f * Mathf.Pi; 
		
		//bullet.LinearVelocity = velocity.Rotated(angle);

		// Spawn the mob by adding it to the Main scene.
		AddChild(bullet);
		
		bullet.AddToGroup(Constants.ProjectileGroup);
	}
}
