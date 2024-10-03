using Godot;
using System;
using System.Threading;

public partial class TrackingMob : RigidBody2D, IDamager, IKillable
{
	public Node2D Target;

	[Export]
	public EnemyType EnemyType;

	[Export]
	public float Damage { get; set; } = 10f;

	[Export]
	public float Health { get; set; } = 10f;

	[Export]
	public float Speed { get; set; } = 200f;

	[Signal]
	public delegate void HitEventHandler(int amount, int type);

	[Export]
	public int LootDropRate1InN = 2;

	public float GetDamageAmount() => Damage;
	public void OnDamaged() { }

	public bool IsDead = false;
	public float DieAnimationTime = 0.5f;
	public double DieEndTime = double.NegativeInfinity;

	private Vector2 InternalScale = Vector2.One;

	public AnimationPlayer AnimationPlayer;
	public double GameTime;

	public bool EnteredScreen = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		string[] mobTypes = animatedSprite2D.SpriteFrames.GetAnimationNames();
		animatedSprite2D.Play(mobTypes[GD.Randi() % mobTypes.Length]);
		GameTime = 0;
		EnteredScreen = false;

		var screenNotifier = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");

		screenNotifier.ScreenEntered += () => EnteredScreen = true;
		screenNotifier.ScreenExited += OffScreen;
	}

	public void OnHit(float damage)
	{
		Health -= damage;
		AnimationPlayer?.Play("hit");

		Main.ScoreBoxSpawner.CreateScoreText((int)-damage, HitEventType.Damage, this.Position);
		EmitSignal(SignalName.Hit, damage, (int)HitEventType.Damage);

		if (Health <= 0)
		{
			GetNode<CollisionShape2D>("HitBox/CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
			CallDeferred("Died");
			//Died();
		}
	}

	public void OffScreen()
	{
		if (EnteredScreen)
		{
			QueueFree();
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

	public void RigidBodyCollision(Node node)
	{

	}

	public void Died()
	{
		if (IsDead)
		{
			return;
		}
		IsDead = true;

		// stop colliding
		GetNode<CollisionShape2D>("CollisionShape2D")?.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
		GetNode<CollisionShape2D>("HitBox/CollisionShape2D")?.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);

		Speed = 0;
	}

	public override void _IntegrateForces(PhysicsDirectBodyState2D state)
	{
		base._IntegrateForces(state);
		Vector2 t;
		if (Target?.Position == null)
		{
			t = Vector2.Zero;
		}
		else
		{
			t = Target.Position;
		}

		bool isLeftOfPlayer = Position.X < t.X;

		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite2D.FlipH = isLeftOfPlayer;

		Vector2 vectorTo = (t - this.Position).Normalized();
		this.Rotation = 0f;
		state.LinearVelocity = vectorTo * Speed / state.Step;
		//ApplyForce(vectorTo * Speed);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		GameTime += delta;
		if (IsDead)
		{
			if(DieEndTime == double.NegativeInfinity)
			{
				DieEndTime = GameTime + DieAnimationTime;
			}
			if (InternalScale.Length() > 0.1f)
			{
				InternalScale -= new Vector2(0.1f, 0.1f);
			}
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").Scale = InternalScale;
			if (GameTime >= DieEndTime)
			{
				if (GD.Randi() % LootDropRate1InN == 0)
				{
					SignalManager.Instance.EmitSignal(SignalManager.SignalName.EnemyDied, (int)this.EnemyType, this.Position);
				}
				QueueFree();
			}
		}
	}
}
