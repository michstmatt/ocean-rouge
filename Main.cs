using Godot;
using System;

public partial class Main : Node
{
   	private int _score;
	public static PickupSpawner PickupSpawner;
	public static ScoreBoxSpawner ScoreBoxSpawner;

	public bool IsPaused = false;

	public override void _Ready()
	{
		base._Ready();
		ProcessMode = Node.ProcessModeEnum.Always;
		var mobSpawner = GetNode<TrackingMobSpawner>("TrackingMobSpawner");
		var pickupSpawner = GetNode<PickupSpawner>("PickupSpawner");
		PickupSpawner = pickupSpawner;
		ScoreBoxSpawner = GetNode<ScoreBoxSpawner>("ScoreBoxSpawner");

		SignalManager.Instance.PauseGame += PauseGame;
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		if(Input.IsActionPressed("pause"))
		{
			SignalManager.Instance.EmitSignal(SignalManager.SignalName.PauseGame, !IsPaused);
		}
	}

	public void GameOver()
	{
		GetNode<Timer>("ScoreTimer").Stop();
		GetNode<Hud>("HUD").ShowGameOver();
		GetNode<AudioStreamPlayer>("Music").Stop();
		GetNode<AudioStreamPlayer>("DeathSound").Play();
		SignalManager.Instance.EmitSignal(SignalManager.SignalName.GameOver, true);
	}

	public void NewGame()
	{
		_score = 0;

		var hud = GetNode<Hud>("HUD");
		hud.UpdateScore(_score);
		hud.ShowMessage("Get Ready!");

		SignalManager.Instance.EmitSignal(SignalManager.SignalName.GameOver, false);
		GetTree().CallGroup("mobs", Node.MethodName.QueueFree);
		GetNode<PickupSpawner>("PickupSpawner").Init();
		
		var player = GetNode<Player>("Player");
		player.AddToGroup(Constants.PlayerGroup);
		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);

		GetNode<Timer>("StartTimer").Start();
		GetNode<AudioStreamPlayer>("Music").Play();
		SignalManager.Instance.EmitSignal(SignalManager.SignalName.NewWeaponAvailable);
	}

	public void PauseGame(bool isPaused)
	{
		GetTree().Paused = isPaused;
		if (isPaused)
		{
			GetNode<Timer>("StartTimer").Stop();
			GetNode<Timer>("ScoreTimer").Stop();
		}
		else{
			GetNode<Timer>("StartTimer").Start();
			GetNode<Timer>("ScoreTimer").Start();
		}

		IsPaused = isPaused;
	}
	
	[Signal]
	public delegate void GameStartEventHandler(bool start);
	
	// We also specified this function name in PascalCase in the editor's connection window.
	private void OnScoreTimerTimeout()
	{
		_score++;
		GetNode<Hud>("HUD").UpdateScore(_score);
	}

	// We also specified this function name in PascalCase in the editor's connection window.
	private void OnStartTimerTimeout()
	{
		GetNode<Timer>("ScoreTimer").Start();
		EmitSignal(SignalName.GameStart, true);
	}

}
