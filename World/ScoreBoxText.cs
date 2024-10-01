using Godot;
using System;

public partial class ScoreBoxText : Node2D
{
	public int Amount {get; set;}
	public HitEventType HitEventType { get; set; }

	public Vector2 ShrinkRate {get; set;} = new Vector2(0.5f, 0.5f);

	[Export]
	Color Damage {get; set;}

	[Export]
	Color Health {get; set;}

	[Export]
	Color Coins {get; set;}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var label = GetNode<Label>("Label");
		label.Text = Amount.ToString();
		Color color;
		switch (HitEventType)
		{
			case HitEventType.Damage:
				color = Damage;
				break;
			case HitEventType.Health:
				color = Health;
				break;
			case HitEventType.Coins:
			 	color = Coins;
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
		base._Process(delta);
		this.Scale -= ShrinkRate * (float)delta;
	}

	public void OnTimerTimeout()
	{
		QueueFree();
	}
	
}
