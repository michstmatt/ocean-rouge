using Godot;
using System;

public partial class PickupSpawner : Node
{
	[Export]
   	public PackedScene PickupScene { get; set; }
	public PickupType[] PickupTypes = new PickupType[] {PickupType.Coin, PickupType.Health}; 
	public PickupType[] RarePickupTypes = new PickupType[] {PickupType.Chest};

	[Export]
	public DynamicFloor DynamicFloor {get; set;}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Init();
		SignalManager.Instance.PauseGame += (bool isPaused) =>
		{
			var timer = GetNode<Timer>("RandomPlacementTimer");
			timer.Paused = isPaused;
		};
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

	public void SpawnRandomPickup()
	{
		Vector2I randTile = DynamicFloor.Floor.GetUsedCells().PickRandom();
		var position = DynamicFloor.Floor.MapToLocal(randTile);
		PickupType type;
		if (GD.Randi() % 25 == 0)
		{
			type = RarePickupTypes[(int)GD.Randi() % RarePickupTypes.Length];
		}
		else
		{
			type = PickupTypes[Random.Shared.Next(PickupTypes.Length)];
		}
		uint amount = (GD.Randi() % 5) + 1;
		this.SpawnNewPickup(type, amount, position);
	}

	public void RandomTimerTimeout()
	{
		SpawnRandomPickup();
	}
}
