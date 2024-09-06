using Godot;
using System;

public partial class Main : Node
{
   	private int _score;
	
	public override void _Ready()
	{
		var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
		var mobSpawner = GetNode<TrackingMobSpawner>("TrackingMobSpawner");
		mobSpawner.MobSpawnLocation = mobSpawnLocation;
		
	}
	
	public void GameOver()
	{
		GetNode<Timer>("ScoreTimer").Stop();
		GetNode<Hud>("HUD").ShowGameOver();
		GetNode<AudioStreamPlayer>("Music").Stop();
		GetNode<AudioStreamPlayer>("DeathSound").Play();
		EmitSignal(SignalName.GameStart, false);
	}

	public void NewGame()
	{
		_score = 0;
		var hud = GetNode<Hud>("HUD");
		hud.UpdateScore(_score);
		hud.ShowMessage("Get Ready!");

		GetTree().CallGroup("mobs", Node.MethodName.QueueFree);
		
		var player = GetNode<Player>("Player");
		player.AddToGroup(Constants.PlayerGroup);
		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);

		GetNode<Timer>("StartTimer").Start();
		GetNode<AudioStreamPlayer>("Music").Play();
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

	[Signal]
	public delegate void SpawnMobEventHandler(PathFollow2D path);
}
