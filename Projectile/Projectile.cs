using Godot;
using System;

public partial class Projectile : CharacterArea2D, IDamager
{
	[Export]
	public int Damage = 10;

	[Export]
	public Vector2 Velocity = Vector2.Zero;

	public int GetDamageAmount() => Damage;

	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Health = 10;
		Character.Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		Character.Sprite.Play("default");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
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
		
		Position += Velocity * (float)delta;
	}
	
	private void OnBodyEntered(Node2D body)
	{

		if (body is IKillable)
		{
			(body as IKillable).OnHit(Damage);
			QueueFree();
		}
	}
	
	private void OnVisibleOnScreenNotifier2DScreenExited()
	{
		QueueFree();
	}
}
