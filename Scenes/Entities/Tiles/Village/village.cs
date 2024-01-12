using Castle.Scenes.Entities.Tiles;
using Castle.Static;
using Godot;
using System;

public partial class Village : CharacterBody2D, ITileNode
{
    public Vector2I GridCoords { get; set; }
    public int TeamId { get; set; }

    double MoneyPerSecond = 2f;

    public override void _Process(double delta)
    {
        GlobalData.Player.Gold += MoneyPerSecond * delta;
        base._Process(delta);
    }
}
