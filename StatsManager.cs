using System.Collections.Generic;
using Godot;

public partial class StatsManager : Node
{
	public int EnemiesKilled {get; set;} = 0;
	public int CoinsCollected {get; set;} = 0;

	public override void _Ready()
	{
		EnemiesKilled = 0;
		SignalManager.Instance.EnemyDied += OnEnemyKilled;
	}

	public void OnEnemyKilled(EnemyType t)
	{
		EnemiesKilled += 1;
	}
}
