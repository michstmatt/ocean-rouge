using Godot;
using System;
using System.Threading;

public partial class TrackingMob : RigidBody2D, IDamager, IKillable
{
	public Node2D Target;
	
	public EnemyType EnemyType;
	
	[Signal]
	public delegate void HitEventHandler(int amount, int type);

	public int GetDamageAmount() => EnemyType.Damage;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite2D.Play(EnemyType.AnimationName);
	}
	
	public void OnHit(int damage)
	{
		EnemyType.Health -= damage;

		Main.ScoreBoxSpawner.CreateScoreText(-damage, HitEventType.Damage, this.Position);
		EmitSignal(SignalName.Hit, damage, (int)HitEventType.Damage);

		if(EnemyType.Health <= 0)
		{
			GetNode<CollisionShape2D>("HitBox/CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
			Died();
		}
	}
	
	public void OnBodyEntered(Node2D node)
	{
		if (node is Projectile)
		{
			var projectile = (Projectile)node;
			var damage = projectile.GetDamageAmount();
			CallDeferred("OnHit", damage);
			projectile.CallDeferred("Damaged");
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
		LinearVelocity = vectorTo * EnemyType.Speed;

		this.Scale = Vector2.One * EnemyType.Scale;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
}
