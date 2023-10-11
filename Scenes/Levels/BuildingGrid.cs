using Castle.Scenes.Entities.Tiles;
using Godot;
using System;
using System.Collections.Generic;

[Tool]
public partial class BuildingGrid : Node2D
{
    Dictionary<Vector2I, ITileNode> grid = new();

    int width_backing = 2;
    int height_backing = 2;
    int size_backing = 32;

    [Export]
    public int Width {
        get => width_backing;
        set {
            width_backing = value < 1 ? 1 : value;
            QueueRedraw();
        }
    }

    [Export]
    public int Height {
        get => height_backing;
        set {
            height_backing = value < 1 ? 1 : value;
            QueueRedraw();
        }
    }

    [Export]
    public int Size {
        get => size_backing;
        set {
            size_backing = value < 1 ? 1 : value;
            QueueRedraw();
        }
    }

    public enum HoverType { 
        None = 0,
        Invalid = 1,
        Placeable = 2,
    }

    static Color InvalidColor = Colors.Red;
    static Color PlaceableColor = Colors.Green;

    HoverType Hover;
    Vector2I? HoverPos;

    public Vector2I? PositionToCoords(Vector2 position)
    {
        Vector2I pos = new((int)(position.X / Size), (int)(position.Y / Size));
        if (pos.X < 0 ||
            pos.Y < 0 ||
            pos.X > Width ||
            pos.Y > Height)
        {
            return null;
        }
        return pos;
    }

    public void HoverOver(Vector2? position, out bool can_place)
    {
        var pos = position == null ? null : PositionToCoords(position.Value);

        if (pos == null) 
        {
            HoverPos = null;
            Hover = HoverType.None;
            can_place = false;
            return;
        }

        HoverPos = pos;
        Hover = HoverType.Placeable;
        can_place = true;
        QueueRedraw();
    }

    public void Add(TileStats tile, Vector2 position) => 
        Add(tile, PositionToCoords(position) ?? throw new Exception("Tile cannot be placed at the given position."));
    public void Add(TileStats tile, Vector2I coords) {
        if (grid.ContainsKey(coords)) throw new Exception("Tile space already occupied.");
        var node = tile.Tile.Instantiate<CharacterBody2D>();
        var i_tile = (ITileNode)node;
        grid.Add(coords, i_tile);
        node.Position = coords * Size;
        i_tile.GridCoords = coords;
        AddChild(node);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//QueueRedraw();
	}

    public override void _Draw()
    {
        DrawGrid();

        base._Draw();
    }

    void DrawGrid()
    {
        var total_width = Width * Size;
        var total_height = Height * Size;

        var grid_color = Color.Color8(255, 255, 255, 255);

        // Hovered square
        if (HoverPos.HasValue && Hover != HoverType.None)
        {
            var color = Hover == HoverType.Invalid ? InvalidColor : PlaceableColor;
            DrawRect(new(
                HoverPos.Value.X * Size, 
                HoverPos.Value.Y * Size, 
                Size, 
                Size), color, true);
        }

        // Vertical Lines
        for (int x = 0; x < Width; x++)
        {
            DrawLine(new Vector2(x * Size, 0f), new Vector2(x * Size, total_height), grid_color, 3);
        }

        // Horizontal Lines
        for (int y = 0; y < Height; y++)
        {
            DrawLine(new Vector2(0f, y * Size), new Vector2(total_width, y * Size), grid_color, 3);
        }

        // Outer Rect
        DrawRect(new(Vector2.Zero, new Vector2(total_width, total_height)), grid_color, false, 5);
    }
}
