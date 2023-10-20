using Castle.Static;
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
        var vp = GetViewport();
        var camera = vp.GetCamera2D();
        var mouse_p = vp.GetMousePosition();
        var camera_p = camera.Position;
        var camera_z = camera.Zoom;

        Position = mouse_p;

        var held = Input.IsMouseButtonPressed(MouseButton.Left);

        var grid_p = camera_p + ui.BuildingGrid.Position + (mouse_p - vp.GetVisibleRect().Size / 2) / camera_z;
        

        if (held)
        {
            ui.BuildingGrid.HoverOver(grid_p, out can_place);
            if (GlobalData.Player.Gold < Tile.Cost) can_place = false;
        }
        else
        {
            if (can_place) {
                ui.BuildingGrid.Add(Tile, grid_p);
                GlobalData.Player.Gold -= Tile.Cost;
            }

            ui.BuildingGrid.HoverOver(null, out _);
            QueueFree();
        }
    }
}
