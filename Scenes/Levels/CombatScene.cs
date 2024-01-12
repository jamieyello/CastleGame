using Godot;
using System;

public partial class CombatScene : Node2D
{
	static PackedScene select_rect = GD.Load<PackedScene>("uid://bljppdumdlsyb");

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var ui = GetNode<GameplayUi>("CanvasLayer/GameplayUI");
		var bg = GetNode<BuildingGrid>("World/BuildingGrid");
		ui.BuildingGrid = bg;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        var held = Input.IsMouseButtonPressed(MouseButton.Right);
        if (!HasNode("select_rect") && held)
		{
			var n = select_rect.Instantiate();
			n.Name = "select_rect";
            AddChild(n);
        }
    }
}
