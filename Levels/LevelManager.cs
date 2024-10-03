using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using Godot;

public class LevelState
{
	public int NumEnemies { get; set; }
	public int NumRooms { get; set; }
	public float EnemySpawnRate { get; set; }
	public int NumConcurrentEnemies { get; set; }
}
public partial class LevelManager : Node
{
	private List<LevelState> LevelStates;

	private int CurrentLevel;

	private int NumLevels = 20;
	private int EnemyIncrease = 5;
	private LevelState Start = new LevelState()
	{
		NumEnemies = 10,
		NumRooms = 4,
		EnemySpawnRate = 2
	};

	public override void _Ready()
	{
		base._Ready();
		GD.Print("HERE");
		LevelManager.Instance = this;
		Init();
	}

	public static LevelManager Instance { get; private set; }

	public void Init()
	{
		SignalManager.Instance.OnStairsEntered += OnNextLevel;
		SignalManager.Instance.GameOver += (isOver) =>
		{
			if (isOver)
			{
				InitLevels();
			}
		};
		InitLevels();
	}
	public void InitLevels()
	{
		CurrentLevel = 0;
		LevelStates = new List<LevelState>(NumLevels);

		for (int i = 0; i < NumLevels; i++)
		{
			var level = new LevelState()
			{
				NumEnemies = Start.NumEnemies + i * EnemyIncrease,
				NumRooms = Start.NumRooms + (i / 10),
				EnemySpawnRate = Start.EnemySpawnRate,
				NumConcurrentEnemies = 5
			};
			LevelStates.Add(level);
		}
	}

	public void OnNextLevel()
	{
		CurrentLevel++;
		SignalManager.Instance.EmitSignal(SignalManager.SignalName.NextLevel);
	}
	public LevelState GetCurrentLevel()
	{
		return LevelStates[CurrentLevel];
	}
}
