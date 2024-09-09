using Godot;
using System;
using System.ComponentModel;

public enum PickupType
{
	Health,
	Coin
}

public partial class Pickup : Area2D
{
	[Export]
	public PickupType Type {get;set;} = PickupType.Coin;

	[Export]
	public uint Amount {get;set;} = 1;
	
	public void Init(PickupType type, uint amount)
	{
		this.Type = type;
		this.Amount = amount;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		switch (Type)
		{
			case PickupType.Health:
				animatedSprite2D.Play("Heart");
				break;
			case PickupType.Coin:
			default:
				animatedSprite2D.Play("Coin");
				break;
		}
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void OnBodyEntered(Node2D node)
	{	
		if(node is Player)
		{
			var player = (Player)node;
			player.OnItemPickedUp(this.Type, this.Amount);
			QueueFree();
		}
		
	}
}
