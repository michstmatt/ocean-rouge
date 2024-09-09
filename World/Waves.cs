using Godot;
using System;

public partial class Waves : Node2D
{
	TileMapLayer Tiles;

	[Export]
	public Vector2I Dimensions = new Vector2I(10,10);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Tiles = GetNode<TileMapLayer>("TileMapLayer");
		CreateFloor(Tiles);
	}

	public void CreateFloor(TileMapLayer tileMap)
	{
		Vector2I loc = Vector2I.Zero;
		Vector2I idx = Vector2I.Zero;
		
		for(int col = -Dimensions.X; col < Dimensions.X; col +=1)
		{
			loc.X = col;
			for (int row = -Dimensions.Y ; row < Dimensions.Y; row +=1)
			{
				loc.Y = row;
				tileMap.SetCell(loc, 0, idx);
			}
		}
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
