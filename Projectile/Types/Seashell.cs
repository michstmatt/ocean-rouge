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
	}
	
	protected override void PlaySprite()
	{
		string[] mobTypes = Sprite.SpriteFrames.GetAnimationNames();
		Sprite.Play(mobTypes[GD.Randi() % mobTypes.Length]);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		this.Rotation = LastRotation + (RotationSpeed * (float)delta);
		this.LastRotation = this.Rotation;
	}
}
