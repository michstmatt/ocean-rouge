using Godot;
using System;

public partial class Projectile : RigidBody2D, IDamager
{
	[Export]
	public WeaponType WeaponType;

	[Export]
	public WeaponFireType WeaponFireType;

	[Export]
	public int HitCount {get; set;}

	[Export]
	public float Speed {get; set;}

	[Export]
	public float Damage{get; set;} = 10;

	public AnimatedSprite2D Sprite;

	[Export]
	public Vector2 Direction {get; set;} = Vector2.Right;

	public float GetDamageAmount() => Damage;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		Sprite.Play("default");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Direction = Direction.Normalized() * Speed;
		
		Rotation = Direction.Angle();
		//Position += Velocity * (float)delta;
		LinearVelocity = Direction;
	}

	public void OnDamaged()
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
