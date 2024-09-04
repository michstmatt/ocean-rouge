using Godot;
using System;

public partial class Player : AbstractCharacter
{
	public Vector2 ScreenSize; // Size of the game window.
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		Hide();
	}
	
	public void Start(Vector2 position)
	{
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}
	
	private Vector2 GetInput()
	{
		var velocity = Vector2.Zero;
		if (Input.IsActionPressed("move_right"))
		{
			velocity.X += 1;
		}

		if (Input.IsActionPressed("move_left"))
		{
			velocity.X -= 1;
		}

		if (Input.IsActionPressed("move_down"))
		{
			velocity.Y += 1;
		}

		if (Input.IsActionPressed("move_up"))
		{
			velocity.Y -= 1;
		}
		
		return velocity;
	}
	
	private void Animate(Vector2 velocity)
	{
		if (velocity.X != 0)
		{
			sprite.Animation = "walk";
			sprite.FlipV = false;
			// See the note below about the following boolean assignment.
			sprite.FlipH = velocity.X < 0;
		}
		else if (velocity.Y != 0)
		{
			sprite.Animation = "up";
			sprite.FlipV = velocity.Y > 0;
		}
		
		if(velocity.Length() > 0) {sprite.Play();} else{sprite.Stop();}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var velocity = GetInput();

		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
		}
		
		Position += velocity * (float)delta;
		
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
		);
		
		Animate(velocity);
	}
	
	private void OnBodyEntered(Node2D body)
	{

		Health -= 10;
		EmitSignal(SignalName.Hit, Health);
		
		if(Health <= 0)
		{
			Hide();
			EmitSignal(SignalName.Dead);
			GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
		}

	}
}
