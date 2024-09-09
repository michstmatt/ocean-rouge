using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class DynamicLevel : Node2D
{
	TileMapLayer Floor;
	TileMapLayer Decor;

	[Export]
	public Vector2I Dimensions = new Vector2I(300,300);
	public Vector2I FloorTileMapSet = new Vector2I(3,3);
	public Vector2I DecorTileMapSet = new Vector2I(3,3);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Floor = GetNode<TileMapLayer>("Floor");
		Decor = GetNode<TileMapLayer>("Decor");

		CreateFloor(Floor, new [] {0}, FloorTileMapSet, 1);
		CreateFloor(Decor, new [] {1,2}, DecorTileMapSet, 2);
	}

	public void CreateFloor(TileMapLayer tileMap, IEnumerable<int> indices, Vector2I tileMapSet, int rateInX)
	{
		Vector2I loc = Vector2I.Zero;
		Vector2I idx = Vector2I.Zero;
		TileSet tileSet = tileMap.TileSet;
		for(int col = -Dimensions.X; col < Dimensions.X; col +=1)
		{
			loc.X = col;
			for (int row = -Dimensions.Y ; row < Dimensions.Y; row +=1)
			{
				loc.Y = row;

				var atlasIdx = indices.ElementAt((int)(GD.Randi() % indices.Count()));

				if ((int)GD.Randi() % rateInX == 0)
				{
					idx.X =(int)(GD.Randi() % tileMapSet.X);
					idx.Y = (int)(GD.Randi() % tileMapSet.Y);
					tileMap.SetCell(loc, atlasIdx, idx);
				}
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
