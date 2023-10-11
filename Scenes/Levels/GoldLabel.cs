using Castle.Static;
using Godot;
using System;

public partial class GoldLabel : Label
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Text = ((int)GlobalData.Player.Gold).ToString();
	}
}
