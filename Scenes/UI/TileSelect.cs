using Godot;
using System;
using System.Collections.Generic;

public partial class TileSelect : VBoxContainer
{
	[Export]
	public TileStats[] AvailableTiles = new TileStats[0];

	static PackedScene TileDragButton = GD.Load<PackedScene>("uid://d3fv048p2asg6");

	public override void _Ready()
	{
		ResetChildren();
	}

	void ResetChildren()
	{
		foreach (var node in GetChildren())
		{
			RemoveChild(node);
			node.QueueFree();
		}

		foreach (var tile in AvailableTiles)
		{
			var button = TileDragButton.Instantiate<TileDragButton>();
			button.Data = tile;
			AddChild(button);
		}
	}
}
