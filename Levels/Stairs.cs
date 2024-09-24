using Godot;
using System;

public partial class Stairs : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

		this.BodyEntered += (Node2D player) => {
			SignalManager.Instance.EmitSignal(SignalManager.SignalName.OnStairsEntered);
		};
	}
}
