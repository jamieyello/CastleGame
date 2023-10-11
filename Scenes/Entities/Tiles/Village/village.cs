using Castle.Scenes.Entities.Tiles;
using Castle.Static;
using Godot;
using System;

public partial class village : CharacterBody2D, ITileNode
{
    public Vector2I GridCoords { get; set; }

    double MoneyPerSecond = 2f;

    public override void _Process(double delta)
    {
        GlobalData.Player.Gold += MoneyPerSecond * delta;
        base._Process(delta);
    }
}
