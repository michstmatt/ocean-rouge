using Godot;
using System;
using System.Threading;

public partial class TrackingMob : RigidBody2D, IDamager, IKillable
{
	public Node2D Target;

	[Export]	
	public EnemyType EnemyType;

	[Export]
	public float Damage {get; set;} = 10f;

	[Export]
	public float Health {get; set;} = 10f;

	[Export]
	public float Speed {get; set;} = 200f;

	[Signal]
	public delegate void HitEventHandler(int amount, int type);

	public float GetDamageAmount() => Damage;
	public void OnDamaged() {}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite2D.Play("default");
	}
	
	public void OnHit(float damage)
	{
		Health -= damage;

		Main.ScoreBoxSpawner.CreateScoreText((int)-damage, HitEventType.Damage, this.Position);
		EmitSignal(SignalName.Hit, damage, (int)HitEventType.Damage);

		if(Health <= 0)
		{
			GetNode<CollisionShape2D>("HitBox/CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
			Died();
		}
	}
	
	public void OnBodyEntered(Node2D node)
	{
		if (node is IDamager)
		{
			var projectile = (IDamager)node;
			var damage = projectile.GetDamageAmount();
			CallDeferred("OnHit", damage);
			node.CallDeferred("OnDamaged");
		}
	}
	

	public void Died()
	{
		if (GD.Randi() % 10 == 0)
		{
			Main.PickupSpawner.CallDeferred("SpawnRandomPickup", this.Position);
		}
		QueueFree();
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 t;
		if (Target?.Position == null)
		{
			t = Vector2.Zero;
		}else
		{
			t = Target.Position;
		}

		bool isLeftOfPlayer = Position.X < t.X;

		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite2D.FlipH = isLeftOfPlayer;

		var rotation = this.Position.AngleToPoint(t);
		Vector2 vectorTo = Vector2.FromAngle(rotation);
		LinearVelocity = vectorTo * Speed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
}
