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

	private bool IsPaused = false;

	public int raycastIndex = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		AnimationPlayer = GetNode<AnimationPlayer>("HitAnimationPlayer");

		SignalManager.Instance.ItemPickup += OnItemPickedUp;
		SignalManager.Instance.PauseGame += (isPaused) => IsPaused = isPaused;

		GetNode<Timer>("CollisionTimer").Timeout += () => DisableCollision(false);

		Hide();
	}
	public void Start(Vector2 position)
	{
		Health = 100;
		Position = position;
		Show();
		DisableCollision(false);
	}
	
	private Vector2 GetInput()
	{
		if(IsPaused)
		{
			return Vector2.Zero;
		}
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
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		var velocity = GetInput();

		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed * (float)delta;
		}
		
		Velocity = velocity;
		MoveAndCollide(Velocity);

		HandleRayCast();
		//MoveAndSlide();
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		Animate(Velocity);
	}
	
	public void HandleRayCast()
	{
		var raycast = GetNode<RayCast2D>("RayCast2D");
		var rays = 36;
		float angle = 0;
		float distance = 1200;

		angle = raycastIndex * (Mathf.Pi * 2.0f /rays);
		raycast.TargetPosition = Vector2.FromAngle(angle) * distance;
		if (raycast.IsColliding())
		{
			SignalManager.Instance.EmitSignal(SignalManager.SignalName.OnTileSeen, raycast.GetCollisionPoint());
		}
		raycastIndex += 1;
		raycastIndex %= rays;
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
				rb.MoveAndCollide(oppDirectionToPlayer * EnemyBounce);
				
				//rb.ApplyCentralImpulse(oppDirectionToPlayer * EnemyBounce);
				rb.OnHit(CollisionDamage);
				MoveAndCollide( -EnemyBounce * oppDirectionToPlayer);
			}
		} 
	}

	public void DisableCollision(bool disabled = true)
	{
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, disabled);
		GetNode<CollisionShape2D>("Area2D/CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, disabled);
	}

	public void OnHit(float damage)
	{
		AnimationPlayer.Play("hit");
		Health -= damage;
		if (Health <= 0)
		{
			Hide();
			EmitSignal(SignalName.Dead);
			DisableCollision(true);
		}
		else
		{
			DisableCollision(true);
			GetNode<Timer>("CollisionTimer").Start();
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
