using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class MiniMap : Control
{
	// Called when the node enters the scene tree for the first time.

	public DynamicFloor Level { get; set; }

	[Export]
	public float TileSize = 10;

	[Export]
	public Color TileColor = new Color(1, 1, 1, .7f);

	[Export]
	public Color PlayerColor = new Color(0, 0, 1, .7f);

	public List<Player> Players;

	public override void _Ready()
	{
		base._Ready();
	}

	public override void _Draw()
	{
		base._Draw();
		DrawMap();
	}

	public void Redraw()
	{
		QueueRedraw();
	}

	public void DrawMap()
	{
		Players = GetTree().GetNodesInGroup(Constants.PlayerGroup).Select(p => p as Player).ToList();

		if(Level == null)
		{
			Level = GetTree().GetFirstNodeInGroup(Constants.DynamicFloor) as DynamicFloor;
		}

		var usedCells = Level.Floor.GetUsedCellsById(Level.WallAtlasIndices[0]);
		var minX = usedCells.Select(v => v.X).Min();
		var minY = usedCells.Select(v => v.Y).Min();
		var offset = new Vector2(Math.Abs(minX), Math.Abs(minY)) * TileSize;

		var pos = Vector2.Zero;
		foreach (var cell in usedCells)
		{
			pos = offset + new Vector2(cell.X, cell.Y) * TileSize;
			DrawRect(new Rect2(pos, new Vector2(TileSize, TileSize)), TileColor);
		}
		foreach(var player in Players)
		{
			var playerPos = player.GlobalPosition;
			var tileLocation = Level.Floor.LocalToMap(playerPos);
			var corrected = TileSize * new Vector2(tileLocation.X, tileLocation.Y) / Level.Scale.X;
			DrawCircle(corrected + offset, TileSize/2, PlayerColor);
		}
	}
}
