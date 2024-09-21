using Godot;
using System;
using System.Collections.Generic;
public partial class Decorator : Node2D
{

	[Export]
	public int[] DecoreAtlasIndices;

	[Export]
	public Vector2I Dimensions = new Vector2I(10,10);

	[Export]
	public int SpawnNumDecorations = 20;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var decorLayer = GetNode<TileMapLayer>("DecorLayer");
		PlaceDecor(decorLayer);
	}

	public void PlaceDecor(TileMapLayer tileMap)
	{
		Vector2I loc = Vector2I.Zero;

		var availableTiles = new List<(int, Vector2I)>();

		foreach(var sourceId in DecoreAtlasIndices)
		{
			var source = tileMap.TileSet.GetSource(sourceId);
			var sourceLen = source.GetTilesCount();
			for(int tileId = 0; tileId < sourceLen; tileId++)
			{
				availableTiles.Add((sourceId, source.GetTileId(tileId)));
			}
		}

		for (int i = 0; i < SpawnNumDecorations; i++)
		{
			loc.X = (int)GD.Randi() % Dimensions.X;
			loc.Y = (int)GD.Randi() % Dimensions.Y;

			if (loc.X < 0)
			{
				loc.X = -loc.X;
			}

			if (loc.Y < 0)
			{
				loc.Y = - loc.Y;
			}

			// check if there is already decor here
			if (tileMap.GetCellSourceId(loc) != -1)
			{
				i--;
				continue;
			}
			var (atlasId, randomTileIdx) = availableTiles[(int)(GD.Randi() % availableTiles.Count)];
			tileMap.SetCell(loc, atlasId, randomTileIdx);
		}
		
	}

}
