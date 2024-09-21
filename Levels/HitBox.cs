using Godot;
using System;

public enum HitEventType
{
	Damage,
	Health,
	Coins
}

public partial class HitBox : Node2D
{

	Timer animationTimer;
	Label hitText;
	Label healthText;
	Label coinsText;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animationTimer = GetNode<Timer>("Timer");
		hitText = GetNode<Label>("HitText");
		healthText = GetNode<Label>("HealthText");
		coinsText = GetNode<Label>("CoinsText");
		HideText();
	}
	public void HideText()
	{
		hitText.Visible = false;
		healthText.Visible = false;
		coinsText.Visible = false;
	}
	
	public void HitEventHandler(int amount, HitEventType eventType)
	{
		animationTimer.Start();
		
		Label label;
		string precursor = "+";
		if (eventType == HitEventType.Damage)
		{
			label = hitText;
			precursor = "-";
		}
		else if (eventType == HitEventType.Health)
		{
			label = healthText;
		}
		else if (eventType == HitEventType.Coins)
		{
			label = coinsText;
		}
		else
		{
			return;
		}

		label.Visible = true;
		var labelText = $"{precursor}{amount}";
		label.Text = labelText;
	}

	public void TimerTimeout()
	{
		HideText();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
