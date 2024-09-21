using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class DynamicLevel : Node2D
{
	TileMapLayer Floor;
	TileMapLayer Decor;

	TileMapLayer Obstacles;
	
	[Export]
	public int[] FloorAtlasIndices;

	[Export]
	public Vector2 FloorSpawnHeight = new Vector2(-1, 0.5f);

	[Export]
	public int[] DecoreAtlasIndices;

	[Export]
	public int[] ObstacleAtalasIndices;

	[Export]
	public Vector2 ObstacleSpawnHeight = new Vector2(0.5f, 1f);

	[Export]
	public Vector2I Dimensions = new Vector2I(300,300);

	[Export]
	public NoiseTexture2D NoiseTexture2D;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Floor = GetNode<TileMapLayer>("Floor");
		Decor = GetNode<TileMapLayer>("Decor");
		Obstacles = GetNode<TileMapLayer>("Obstacles");

		CreateFloor(Floor, FloorAtlasIndices, FloorSpawnHeight);
		CreateDecor(Decor, DecoreAtlasIndices, 4);
		CreateFloor(Obstacles, ObstacleAtalasIndices, ObstacleSpawnHeight);
	}

	public List<(int, Vector2I)> GetAvailableTiles(TileSet tileSet, IEnumerable<int> indices)
	{
		var availableTiles = new List<(int, Vector2I)>();

		foreach(var sourceId in indices)
		{
			var source = tileSet.GetSource(sourceId);
			var sourceLen = source.GetTilesCount();
			for(int tileId = 0; tileId < sourceLen; tileId++)
			{
				availableTiles.Add((sourceId, source.GetTileId(tileId)));
			}
		}

		return availableTiles;
	}

	public void CreateFloor(TileMapLayer tileMap, IEnumerable<int> indices, Vector2 spawnHeight)
	{
		Vector2I loc = Vector2I.Zero;
		var availableTiles = GetAvailableTiles(tileMap.TileSet, indices);
		var noise = NoiseTexture2D.Noise;
		
		for(loc.X = -Dimensions.X; loc.X < Dimensions.X; loc.X +=1)
		{
			for (loc.Y = -Dimensions.Y ; loc.Y < Dimensions.Y; loc.Y +=1)
			{
				var noiseVal = noise.GetNoise2D(loc.X, loc.Y);
				if(noiseVal >= spawnHeight.X && noiseVal < spawnHeight.Y)
				{
					var (atlasId, randomTileIdx) = availableTiles.ElementAt((int)(GD.Randi() % availableTiles.Count()));
					tileMap.SetCell(loc, atlasId, randomTileIdx);
				}
			}
		}
	}
	public void CreateDecor(TileMapLayer tileMap, IEnumerable<int> indices, int rateInX)
	{
		Vector2I loc = Vector2I.Zero;
		var availableTiles = GetAvailableTiles(tileMap.TileSet, indices);
		
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
