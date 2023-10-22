using Godot;
using System;

public partial class combat_scene : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var ui = GetNode<gameplay_ui>("CanvasLayer/GameplayUI");
		var bg = GetNode<BuildingGrid>("World/BuildingGrid");
		ui.BuildingGrid = bg;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

    }
}
