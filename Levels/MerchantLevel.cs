using Godot;
using System;

public partial class MerchantLevel : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);
	}

	public void OnStoreEntered(Node2D node)
	{

	}

	public void OnStairsEntered(Node2D node)
	{
		
	}

}
