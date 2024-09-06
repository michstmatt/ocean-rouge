using Godot;
using System;

public partial class Player : CharacterArea2D 
{
	
	public Vector2 ScreenSize; // Size of the game window.
	public Vector2 LastMove = new Vector2(1,0);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		Character.Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		Hide();
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

		if(Input.IsAnythingPressed())
		{
			LastMove = velocity;
		}
		
		return velocity;
	}
	
	private void Animate(Vector2 velocity)
	{
		if (velocity.X != 0)
		{
			Character.Sprite.Animation = "walk";
			Character.Sprite.FlipV = false;
			// See the note below about the following boolean assignment.
			Character.Sprite.FlipH = velocity.X < 0;
		}
		else if (velocity.Y != 0)
		{
			Character.Sprite.Animation = "up";
			Character.Sprite.FlipV = velocity.Y > 0;
		}
		
		if(velocity.Length() > 0) {Character.Sprite.Play();} else{Character.Sprite.Stop();}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var velocity = GetInput();

		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Character.Speed;
		}
		
		Position += velocity * (float)delta;
		
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
		);
		
		Animate(velocity);
	}
	
	public override void OnHit(int damage)
	{
		base.OnHit(damage);
		if (Character.Health <= 0)
		{
			GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
		}
	}

}
