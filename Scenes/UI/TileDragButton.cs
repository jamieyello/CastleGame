using Castle.Static;
using Godot;
using System;

public partial class TileDragButton : Button
{
	[Export]
	public TileStats Data;

	bool held;

	static PackedScene TileDragVisual = GD.Load<PackedScene>("uid://dun71rbat3n8r");

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var image = GetNode<TextureRect>("PreviewSprite");
		var name_label = GetNode<Label>("NameLabel");
		var description_label = GetNode<Label>("DescriptionLabel");
		var cost_label = GetNode<Label>("CostLabel");

		if (Data != null)
		{
            image.Texture = Data.Preview;
            name_label.Text = Data.TileName;
            description_label.Text = Data.Description;
			cost_label.Text = Data.Cost.ToString();
        }

        Disabled = GlobalData.Player.Gold < Data.Cost;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		Disabled = GlobalData.Player.Gold < Data.Cost;
	}

	void _OnButtonDown()
	{
		held = true;
	}

	void _OnButtonUp()
	{
		held = false;
	}

	void _OnMouseExited()
	{
		if (held)
		{
            var ui = GetTree().Root.GetNode<Node>("CombatScene/CanvasLayer/GameplayUI");
			if (ui.HasNode("TileDragVisual")) return;
			var tile_drag_visual = TileDragVisual.Instantiate<TileDragVisual>();
			tile_drag_visual.Tile = Data;
			ui.AddChild(tile_drag_visual);
		}
	}
}
