using Godot;
using System;

public partial class Projectile : RigidBody2D, IDamager
{
	[Export]
	public int Damage = 10;

	[Export]
	public int HitCount = 2;

	[Export]
	public float Speed = 800;

	[Export]
	public Vector2 Velocity = Vector2.Zero;

	public AnimatedSprite2D Sprite;

	public int GetDamageAmount() => Damage;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		HitCount = 2;
		Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		Sprite.Play("default");
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		var length = Velocity.Length();

		if (length == 0)
		{
			Velocity = Vector2.Right;
		}

		if (length > 0)
		{
			Velocity = Velocity.Normalized() * Speed;
		}
		//Position += Velocity * (float)delta;
		LinearVelocity = Velocity;
	}

	public void Damaged()
	{
		HitCount -= 1;
		if (HitCount <= 0)
		{
			QueueFree();
		}
	}
	private void OnVisibleOnScreenNotifier2DScreenExited()
	{
		QueueFree();
	}
}
