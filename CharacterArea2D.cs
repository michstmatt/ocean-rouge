using Godot;

public partial class CharacterArea2D: Area2D, IKillable
{
	[Signal]
	public delegate void HitEventHandler(int health);
	
	[Signal]
	public delegate void DeadEventHandler();
	
	[Signal]
	public delegate void StartStopEventHandler(bool start);

	[Export]
	public int Health = 100;

	[Export]
	public int Speed = 400;

	public Character Character;
	
	public CharacterArea2D()
	{
		Character = new Character{
			Health = this.Health,
			Speed = this.Speed
		};
	}

	public void Start(Vector2 position)
	{
		Character.Health = Health;
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is IDamager)
		{
			var damage = (body as IDamager).GetDamageAmount();
			OnHit(damage);
		}
	}

	public virtual void OnHit(int damage)
	{
		Character.Health -= damage;
		EmitSignal(SignalName.Hit, Character.Health);
		
		if(Character.Health <= 0)
		{
			Hide();
			EmitSignal(SignalName.Dead);
		}
	}
}
