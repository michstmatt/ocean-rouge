using Godot;
using System;
using System.ComponentModel.DataAnnotations;

public partial class HealthBar : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var healthBar = GetNode<ColorRect>("Fill");
		var health = GetParent<Player>().Health;
		health = health > 100 ? 100 : health;
		healthBar.Scale = new Vector2(.01f * health, healthBar.Scale.Y);
	}
}
