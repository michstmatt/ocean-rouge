using Godot;
using System;

public partial class ScoreBoxText : Node2D
{
	public int Amount {get; set;}
	public HitEventType HitEventType { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var label = GetNode<Label>("Label");
		label.Text = Amount.ToString();
		Color color;
		switch (HitEventType)
		{
			case HitEventType.Damage:
				color = new Color(255,0,0);
				break;
			case HitEventType.Health:
				color = new Color(0,255,0);
				break;
			case HitEventType.Coins:
				color = new Color(255,167,11);
				break;
			default:
				color = new Color(255,255,255);
				break;
		}
		label.AddThemeColorOverride("font_color", color);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public void OnTimerTimeout()
	{
		QueueFree();
	}
	
}
