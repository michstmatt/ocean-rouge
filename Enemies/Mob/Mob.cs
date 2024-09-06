using Godot;
using System;

public partial class Mob : RigidBody2D, IDamager, IKillable
{
	[Export]
	public int Health = 10;

	[Export]
	public int Damage = 10;

	public int GetDamageAmount() => Damage;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		string[] mobTypes = animatedSprite2D.SpriteFrames.GetAnimationNames();
		animatedSprite2D.Play(mobTypes[GD.Randi() % mobTypes.Length]);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void OnVisibleOnScreenNotifier2DScreenExited()
	{
		QueueFree();
	}

	public void OnHit(int damage)
	{
		Health -= damage;

		if(Health <= 0)
		{
			QueueFree();
		}
	}
}
