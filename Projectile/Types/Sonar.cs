using Godot;
using System;
using System.Threading.Tasks;

public partial class Sonar : Projectile
{
	
	[Export]
	public float MaxScale = 25.0f;
	
	[Export]
	public float ScaleRate = 10.0f;

	private float CurrentScale = 1f;
	private float StartDamage;
	private float StartRadius;
	private CircleShape2D CircleShape;
	public override void _Ready()
	{
		base._Ready();
		CurrentScale = 1f;
		StartDamage = Metadata.DamageAmount;
		StartRadius = 11f;
		CircleShape = GetNode<CollisionShape2D>("CollisionShape2D").Shape as CircleShape2D;
		CircleShape.Radius = StartRadius;
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		if(CurrentScale < MaxScale)
		{
			var fDelta = ScaleRate * (float)delta;
			CurrentScale += fDelta;
			Metadata.DamageAmount = Math.Max(StartDamage * (1 - (CurrentScale/MaxScale)), 1f);
			Sprite.Scale = new Vector2(CurrentScale, CurrentScale);
			if (CircleShape != null)
			{
				CircleShape.Radius = StartRadius * CurrentScale;
			}
		}
		else
		{
			ShouldDelete = true;
		}
	}

}
