using Godot;
using System;

public partial class Stairs : Area2D
{
	// Called when the node enters the scene tree for the first time.
	bool locked = true;
	public override void _Ready()
	{
		base._Ready();

		this.BodyEntered += (Node2D player) => {
			if (locked == false)
			{
				SignalManager.Instance.EmitSignal(SignalManager.SignalName.OnStairsEntered);
			}
		};
	
		SignalManager.Instance.NextLevel += (_) => UpdateStairs(true);

		SignalManager.Instance.AllEnemiesDead += () => UpdateStairs(false);
		UpdateStairs(true);
	}

	public void UpdateStairs(bool isLocked)
	{
		var sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		if (isLocked)
		{
			locked = true;
			sprite.Play("locked");
		}
		else
		{
			locked = false;
			sprite.Play("unlocked");
		}
	}
}
