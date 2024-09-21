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

	[Signal]
	public delegate void PauseGameEventHandler(bool pause);

	[Signal]
	public delegate void NewWeaponAvailableEventHandler();

	public static SignalManager Instance {get; private set;}

	public override void _Ready()
	{
		SignalManager.Instance = this;
	}
}
