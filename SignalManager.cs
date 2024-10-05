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
	public delegate void EnemyDiedEventHandler(EnemyType type, Vector2 location);

	[Signal]
	public delegate void PauseGameEventHandler(bool pause);

	[Signal]
	public delegate void NewWeaponAvailableEventHandler();

	[Signal]
	public delegate void OnStairsEnteredEventHandler();

	[Signal]
	public delegate void NextLevelEventHandler(int currentLevel);

	public static SignalManager Instance {get; private set;}

	public override void _Ready()
	{
		SignalManager.Instance = this;
	}
}
