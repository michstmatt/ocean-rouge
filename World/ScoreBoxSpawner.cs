using Godot;
using System;

public partial class ScoreBoxSpawner : Node
{
	[Export]
   	public PackedScene ScoreScene { get; set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void CreateScoreText(int amount, HitEventType type, Vector2 position)
	{
		ScoreBoxText scoreBox = ScoreScene.Instantiate<ScoreBoxText>();
		scoreBox.Amount = amount;
		scoreBox.HitEventType = type;
		scoreBox.Position = position;
		AddChild(scoreBox);
	}
}
