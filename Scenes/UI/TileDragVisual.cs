using Godot;
using System;

public partial class TileDragVisual : Control
{
	[Export]
	public TileStats Tile;

    gameplay_ui ui;

    bool can_place;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        ui = GetParent<gameplay_ui>();
        GetNode<TextureRect>("TextureRect").Texture = Tile.Preview;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position = GetViewport().GetMousePosition();

		var held = Input.IsMouseButtonPressed(MouseButton.Left);

        if (held)
        {
            ui.BuildingGrid.HoverOver(Position, out can_place);
        }
        else
        {
            if (can_place) {
                ui.BuildingGrid.Add(Tile, Position);
            }

            ui.BuildingGrid.HoverOver(null, out _);
            QueueFree();
        }
    }
}
