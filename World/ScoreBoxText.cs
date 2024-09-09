using Godot;
using System;

public partial class ScoreBoxText : Node2D
{
	public int Amount {get; set;}
	public HitEventType HitEventType { get; set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var label = GetNode<Label>("Label");
		label.Text = Amount.ToString();
	}

	public void OnTimerTimeout()
	{
		QueueFree();
	}
	
}
