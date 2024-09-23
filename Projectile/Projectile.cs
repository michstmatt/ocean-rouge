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
	public float Speed {get; set;} = 400f;

	[Export]
	public float Damage{get; set;} = 10;

	public AnimatedSprite2D Sprite;

	[Export]
	public Vector2 Direction {get; set;} = Vector2.Right;

	protected bool ShouldDelete = false;

	public float GetDamageAmount() => Damage;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		Sprite.Play("default");
		
		//Position += Velocity * (float)delta;
	}


	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		Direction = Direction.Normalized() * Speed;
		Rotation = Direction.Angle();
		LinearVelocity = Direction;
		if (ShouldDelete)
		{
			QueueFree();
		}
	}

	public void OnDamaged()
	{
		HitCount -= 1;
		if (HitCount <= 0)
		{
			ShouldDelete = true;
		}
	}
	private void OnVisibleOnScreenNotifier2DScreenExited()
	{
		QueueFree();
	}
}
