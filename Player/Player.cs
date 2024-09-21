using Godot;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody2D, IKillable
{
	
	public Vector2 LastMove = new Vector2(1,0);

	public int Coins = 0;

	public AnimatedSprite2D Sprite;
	public AnimationPlayer AnimationPlayer;

	[Export]
	public float Health = 100;

	[Export]
	public float MaxHealth = 100;

	[Export]
	public float Speed = 400;

	[Export]
	public float EnemyBounce = 50f;

	[Export]
	public float CollisionDamage = 5f;
	
	[Signal]
	public delegate void HitEventHandler(int amount, int type);
	
	[Signal]
	public delegate void DeadEventHandler();

	[Signal]
	public delegate void WeaponPickupEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		AnimationPlayer = GetNode<AnimationPlayer>("HitAnimationPlayer");

		SignalManager.Instance.ItemPickup += OnItemPickedUp;

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
		var velocity = Input.GetVector("move_left", "move_right", "move_up", "move_down");

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
				
				rb.ApplyCentralImpulse(oppDirectionToPlayer * EnemyBounce);
				rb.OnHit(CollisionDamage);
			}
		}
	}

	public void OnHit(float damage)
	{
		AnimationPlayer.Play("hit");
		Health -= damage;
		if (Health <= 0)
		{
			Hide();
			EmitSignal(SignalName.Dead);
			GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
			GetNode<CollisionShape2D>("Area2D/CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
		}
		Main.ScoreBoxSpawner.CreateScoreText((int)-damage, HitEventType.Damage, this.Position);
	}

	public void OnItemPickedUp(PickupType pickupType, uint amount)
	{
		if(pickupType == PickupType.Health)
		{
			this.Health += (int)amount;
			this.Health = Math.Min(this.Health, this.MaxHealth);
			Main.ScoreBoxSpawner.CreateScoreText((int)amount, HitEventType.Health, this.Position);
		}
		else if (pickupType == PickupType.Coin)
		{
			Main.ScoreBoxSpawner.CreateScoreText((int)amount, HitEventType.Coins, this.Position);
			this.Coins += (int)amount;
		}
	}


}
