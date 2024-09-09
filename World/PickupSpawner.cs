using Godot;
using System;

public partial class PickupSpawner : Node
{
	[Export]
   	public PackedScene PickupScene { get; set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Init();
	}

	public void Init()
	{
		GetTree().CallGroup(Constants.PickupGroup, Node.MethodName.QueueFree);
	}
	
	public void SpawnNewPickup(PickupType type, uint amount, Vector2 position)
	{
		Pickup pickup = PickupScene.Instantiate<Pickup>();
		pickup.Init(type, amount);
		pickup.Position = position;
		AddChild(pickup);
		pickup.AddToGroup(Constants.PickupGroup);
	}

	public void SpawnRandomPickup(Vector2 position)
	{
		PickupType type = GD.Randi() % 2 == 0 ? PickupType.Health : PickupType.Coin;
		uint amount = (GD.Randi() % 5) + 1;
		this.SpawnNewPickup(type, amount, position);
	}
}
