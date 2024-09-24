using Godot;
using System;

public partial class Seashell : Projectile
{
	[Export]
	public float RotationSpeed = 1f;
	public float LastRotation = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		string[] mobTypes = animatedSprite2D.SpriteFrames.GetAnimationNames();
		animatedSprite2D.Play(mobTypes[GD.Randi() % mobTypes.Length]);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		this.Rotation = LastRotation + (RotationSpeed * (float)delta);
		this.LastRotation = this.Rotation;
	}
}
