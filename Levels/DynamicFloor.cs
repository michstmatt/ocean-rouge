using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
public partial class DynamicFloor : Node2D
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	public Vector2I Dimensions = new Vector2I(100, 100);

	[Export]
	int MinRoomSize = 4;

	[Export]
	int MaxRoomSize = 10;

	[Export]
	public int MinRoomDistance = 12;

	[Export]
	public int MaxRoomDistance = 24;

	[Export]
	public int NumRooms = 5;

	[Export]
	public int[] FloorAtlasIndices;

	[Export]
	public int[] WallAtlasIndices;

	[Export]
	public int[] DecorAtlasIndices;

	public TileMapLayer Floor;

	public List<(int, Vector2I)> FloorTiles;
	public List<(int, Vector2I)> WallTiles;

	protected Random rng = new Random();

	public Vector2I[] Directions = new Vector2I[] { Vector2I.Down, Vector2I.Left, Vector2I.Up, Vector2I.Right };

	public override void _Ready()
	{
		base._Ready();

		Floor = GetNode<TileMapLayer>("Floor");
		FloorTiles = GetAvailableTiles(Floor.TileSet, FloorAtlasIndices);
		WallTiles = GetAvailableTiles(Floor.TileSet, WallAtlasIndices);
		Setup();
	}

	public void Setup()
	{
		var decor = GetNode<TileMapLayer>("Decor");
		var outside = GetNode<TileMapLayer>("Outside");

		Vector2I loc = Vector2I.Zero;
		for (loc.X = -Dimensions.X; loc.X < Dimensions.X; loc.X += 1)
		{
			for (loc.Y = -Dimensions.Y; loc.Y < Dimensions.Y; loc.Y += 1)
			{
				foreach(var tileMapLayer in new [] {Floor, decor, outside})
				{
					tileMapLayer.SetCell(loc, -1, null);
				}
			}
		};

		CreateDecor(decor, DecorAtlasIndices, 5);
		CreateOutside(outside);
		MakeRooms();
	}

	public int RandNum(int max)
	{
		int res = (int)GD.Randi() % max;
		return Math.Abs(res);
	}

	public List<(int, Vector2I)> GetAvailableTiles(TileSet tileSet, IEnumerable<int> indices)
	{
		var availableTiles = new List<(int, Vector2I)>();

		foreach (var sourceId in indices)
		{
			var source = tileSet.GetSource(sourceId);
			var sourceLen = source.GetTilesCount();
			for (int tileId = 0; tileId < sourceLen; tileId++)
			{
				availableTiles.Add((sourceId, source.GetTileId(tileId)));
			}
		}

		return availableTiles;
	}

	public void CreateOutside(TileMapLayer tileMapLayer)
	{
		Vector2I loc = Vector2I.Zero;

		for (loc.X = -Dimensions.X; loc.X < Dimensions.X; loc.X += 1)
		{
			for (loc.Y = -Dimensions.Y; loc.Y < Dimensions.Y; loc.Y += 1)
			{
				tileMapLayer.SetCell(loc, 0, Vector2I.One);
			}
		};
	}

	public void CreateDecor(TileMapLayer tileMap, IEnumerable<int> indices, int rateInX)
	{
		Vector2I loc = Vector2I.Zero;
		var availableTiles = GetAvailableTiles(tileMap.TileSet, indices);

		for (loc.X = -Dimensions.X; loc.X < Dimensions.X; loc.X += 1)
		{
			for (loc.Y = -Dimensions.Y; loc.Y < Dimensions.Y; loc.Y += 1)
			{
				if ((int)GD.Randi() % rateInX == 0)
				{
					var (atlasId, randomTileIdx) = availableTiles.ElementAt((int)(GD.Randi() % availableTiles.Count()));
					tileMap.SetCell(loc, atlasId, randomTileIdx);
				}
			}
		}
	}

	public void MakeRooms()
	{
		var location = Vector2I.Zero;

		var numRooms = rng.Next(4, 10 + 1);
		MakeRoom(Vector2I.Zero);

		var direction = Vector2I.Zero;

		for (int room = 1; room <= numRooms; room++)
		{
			direction = getNewDirection(direction);
			location = MakeHallway(direction, location);
			MakeRoom(location);
		}
	}

	protected void setEmptyCellItem(Vector2I coords, (int, Vector2I) tileData, int alt = 0)
	{
		var data = Floor.GetCellAtlasCoords(coords);
		var isSet = Floor.GetCellSourceId(coords);
		if (FloorAtlasIndices.Contains(isSet))
		{
			return;
		}
		else if (WallAtlasIndices.Contains(isSet))
		{
			var data2 = FloorTiles[0];
			Floor.SetCell(coords, data2.Item1, data2.Item2);
			return;
		}
		Floor.SetCell(coords, tileData.Item1, tileData.Item2, alt);
	}

	protected Vector2I getNewDirection(Vector2I current)
	{
		Vector2I newDirection = current;

		while (newDirection == current || newDirection == current * -1)
		{
			newDirection = Directions[RandNum(Directions.Length)];
		}

		return newDirection;
	}

	protected void MakeRoom(Vector2I location)
	{
		var width = rng.Next(MinRoomSize, MaxRoomSize + 1) / 2 + 1;
		var height = rng.Next(MinRoomSize, MaxRoomSize + 1) / 2 + 1;

		var stairs = GetNode<Stairs>("Stairs");
		stairs.Position = (location * 32) - new Vector2I(16,16);

		Vector2I current;

		for (int xD = -width; xD <= width; xD++)
			for (int yD = -height; yD <= height; yD++)
			{

				current = location + new Vector2I(xD, yD);

				if (xD == -width)
				{
					if (yD == -height)
					{
						// top left corner wall
						setEmptyCellItem(current, WallTiles[1], 0); // wall
					}
					else if (yD == height)
					{
						// bottom left corner wall
						setEmptyCellItem(current, WallTiles[1], 2); // wall
					}
					else
					{
						// left wall
						setEmptyCellItem(current, WallTiles[0], 2); // wall
					}
				}
				else if (xD == width)
				{
					if (yD == -height)
					{
						// top right corner wall
						setEmptyCellItem(current, WallTiles[1], 1); // wall
					}
					else if (yD == height)
					{
						// bottom right corner wall
						setEmptyCellItem(current, WallTiles[1], 3); // wall
					}
					else
					{
						// right wall
						setEmptyCellItem(current, WallTiles[0], 3); // wall
					}
				}
				else if (yD == -height)
				{
					// top wall
					setEmptyCellItem(current, WallTiles[0]); // wall
				}
				else if (yD == height)
				{
					// bottom wall
					setEmptyCellItem(current, WallTiles[0], 1); // wall
				}
				else
				{
					var floorTile = FloorTiles[RandNum(FloorTiles.Count)];
					setEmptyCellItem(current, floorTile); // floor
				}
			}

	}

	protected Vector2I MakeHallway(Vector2I direction, Vector2I location)
	{
		var length = rng.Next(MinRoomDistance, MaxRoomDistance + 1);
		var wall = WallTiles[0];

		var offset = new Vector2I(direction.Y, direction.X);
		for (int d = 1; d < length-1; d++)
		{

			var floor = FloorTiles[RandNum(FloorTiles.Count)];
			if (direction.X == 0)
			{
				setEmptyCellItem(location + Vector2I.Left, wall, 2);
				setEmptyCellItem(location + Vector2I.Right, wall, 3);
			}
			else
			{
				setEmptyCellItem(location + Vector2I.Up, wall);
				setEmptyCellItem(location + Vector2I.Down, wall, 1);
			}
			setEmptyCellItem(location, floor);

			location += direction;


		}
		return location;
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);
	}
}
