using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class DynamicLevel : Node2D
{
	TileMapLayer Floor;
	TileMapLayer Decor;
	
	[Export]
	public int[] FloorAtlasIndices;


	[Export]
	public int[] DecoreAtlasIndices;

	[Export]
	public Vector2I Dimensions = new Vector2I(300,300);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Floor = GetNode<TileMapLayer>("Floor");
		Decor = GetNode<TileMapLayer>("Decor");

		CreateFloor(Floor, FloorAtlasIndices, 1);
		CreateFloor(Decor, DecoreAtlasIndices, 4);
	}

	public void CreateFloor(TileMapLayer tileMap, IEnumerable<int> indices, int rateInX)
	{
		Vector2I loc = Vector2I.Zero;

		var availableTiles = new List<(int, Vector2I)>();

		foreach(var sourceId in indices)
		{
			var source = tileMap.TileSet.GetSource(sourceId);
			var sourceLen = source.GetTilesCount();
			for(int tileId = 0; tileId < sourceLen; tileId++)
			{
				availableTiles.Add((sourceId, source.GetTileId(tileId)));
			}
		}
		
		for(loc.X = -Dimensions.X; loc.X < Dimensions.X; loc.X +=1)
		{
			for (loc.Y = -Dimensions.Y ; loc.Y < Dimensions.Y; loc.Y +=1)
			{
				if ((int)GD.Randi() % rateInX == 0)
				{
					var (atlasId, randomTileIdx) = availableTiles.ElementAt((int)(GD.Randi() %availableTiles.Count()));
					tileMap.SetCell(loc, atlasId, randomTileIdx);
				}
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
