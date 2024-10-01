using Godot;
using System;

public partial class Projectile : RigidBody2D, IDamager
{
	[Export]
	public WeaponType WeaponType;

	[Export]
	public WeaponFireType WeaponFireType;

	public WeaponMetadata Metadata;

	public AnimatedSprite2D Sprite;

	[Export]
	public Vector2 Direction {get; set;} = Vector2.Right;

	protected bool ShouldDelete = false;

	public float GetDamageAmount() => Metadata.DamageAmount;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		Metadata = WeaponFactory.GetWeaponMetadata(WeaponType).Instantiate();

		Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		PlaySprite();

		var hitbox = GetNode<Area2D>("HitBox");
		if (hitbox != null)
		{
			hitbox.BodyEntered += OnWallHit;
		}
	}
	
	protected virtual void PlaySprite()
	{
		Sprite.Play("default");
	}

	public void OnWallHit(Node2D wall)
	{
		ShouldDelete = true;
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		Direction = Direction.Normalized() * Metadata.Speed;
		Rotation = Direction.Angle();
		LinearVelocity = Direction;
		if (ShouldDelete)
		{
			QueueFree();
		}
	}

	public void OnDamaged()
	{
		Metadata.HitCount -= 1;
		if (Metadata.HitCount <= 0)
		{
			ShouldDelete = true;
		}
	}
	private void OnVisibleOnScreenNotifier2DScreenExited()
	{
		QueueFree();
	}
}
