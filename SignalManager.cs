using Godot;
using System;
using System.Collections.Generic;


public partial class SignalManager : Node
{
	[Signal]
	public delegate void ItemPickupEventHandler(PickupType type, uint amount);
	
	[Signal]
	public delegate void GameOverEventHandler(bool isOver);

	[Signal]
	public delegate void EnemyDiedEventHandler(EnemyType type);

	public static SignalManager Instance {get; private set;}

	public override void _Ready()
	{
		SignalManager.Instance = this;
	}


}
