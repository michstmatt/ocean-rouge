using Godot;
using System;

public partial class TrackingMob : RigidBody2D, IDamager, IKillable
{
	public CharacterArea2D Target;
	
	[Export]
	public int Health = 10;
	
	[Export]
	public int Damage = 10;

	public int GetDamageAmount() => Damage;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite2D.Play("fly");
	}
	
	public void OnHit(int damage)
	{
		Health -= damage;

		if(Health <= 0)
		{
			QueueFree();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 t;
		if (Target?.Position == null)
		{
			t = Vector2.Zero;
		}else
		{
			t = Target.Position;
		}
		
		var rotation = this.Position.AngleToPoint(t);
		Vector2 vectorTo = Vector2.FromAngle(rotation);

		this.Rotation = rotation;
		LinearVelocity = vectorTo * (float)(50);
	}
}
