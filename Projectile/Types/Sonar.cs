using Godot;
using System;

public partial class Sonar : Projectile
{
	
	[Export]
	public float MaxScale = 4.0f;

	public Vector2 MyScale = Vector2.One;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{

		if (MyScale.X < MaxScale)
		{
			var fDelta = 2*(float)delta;
			MyScale += new Vector2(fDelta, fDelta);
			Damage -= (float)delta;
		}

		Scale = MyScale;
		base._PhysicsProcess(delta);
	}
	
	public void Disappear()
	{
		QueueFree();
	}
}
