using Godot;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody2D, IKillable
{
	
	public Vector2 LastMove = new Vector2(1,0);

	public int Coins = 0;

	public AnimatedSprite2D Sprite;

	[Export]
	public float Health = 100;
	[Export]
	public float Speed = 400;

	[Export]
	public float EnemyBounce = 5000f;
	
	[Signal]
	public delegate void HitEventHandler(int amount, int type);
	
	[Signal]
	public delegate void ScoreUpdateEventHandler(int health, int coins);
	
	[Signal]
	public delegate void DeadEventHandler();

	[Signal]
	public delegate void WeaponPickupEventHandler();


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		Hide();
	}
	public void Start(Vector2 position)
	{
		Health = 100;
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
		GetNode<CollisionShape2D>("Area2D/CollisionShape2D").Disabled = false;
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

		if(velocity.X != 0 || velocity.Y != 0)
		{
			LastMove = velocity;
		}
		
		return velocity;
	}
	
	private void Animate(Vector2 velocity)
	{
		if (velocity.X != 0)
		{
			Sprite.Animation = "Swim";
			Sprite.FlipV = false;
			// See the note below about the following boolean assignment.
			Sprite.FlipH = velocity.X < 0;
		}
		else if (velocity.Y != 0)
		{
			Sprite.Animation = "Swim";
			Sprite.FlipV = velocity.Y > 0;
		} else if (velocity.X == 0 && velocity.Y == 0)
		{
			Sprite.Animation = "Default";
		}
		
		if(velocity.Length() > 0) {Sprite.Play();} else{Sprite.Stop();}
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
		
		Animate(velocity);
	}

	public void OnBodyEntered(Node2D body)
	{
		if (body is IDamager)
		{
			var damage = (body as IDamager).GetDamageAmount();
			OnHit(damage);
			if(body is TrackingMob)
			{
				var rb =(body as TrackingMob);
				Vector2 oppDirectionToPlayer = (rb.GlobalPosition-GlobalPosition).Normalized();
				rb.BounceBack = true;
				rb.ApplyCentralImpulse(oppDirectionToPlayer * EnemyBounce);
			}
		}
	}

	public void OnHit(float damage)
	{
		Health -= damage;
		if (Health <= 0)
		{
			Hide();
			EmitSignal(SignalName.Dead);
			GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
			GetNode<CollisionShape2D>("Area2D/CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
		}
		Main.ScoreBoxSpawner.CreateScoreText((int)-damage, HitEventType.Damage, this.Position);
		EmitSignal(SignalName.ScoreUpdate, Health, Coins);
	}

	public void OnItemPickedUp(PickupType pickupType, uint amount)
	{
		if(pickupType == PickupType.Health)
		{
			this.Health += (int)amount;
			Main.ScoreBoxSpawner.CreateScoreText((int)amount, HitEventType.Health, this.Position);
		}
		else if (pickupType == PickupType.Coin)
		{
			Main.ScoreBoxSpawner.CreateScoreText((int)amount, HitEventType.Coins, this.Position);
			this.Coins += (int)amount;
		}
		EmitSignal(SignalName.ScoreUpdate, Health, Coins);
	}


}
