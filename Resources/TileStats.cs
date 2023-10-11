using Godot;
using System;

[GlobalClass]
public partial class TileStats : Resource
{
    [Export]
    public string TileName { get; set; }
    [Export]
    public string Description { get; set; }
    [Export]
    public int BaseHp { get; set; }
    [Export]
    public int Cost { get; set; }

    [Export]
    public Texture2D Preview { get; set; }
    [Export] 
    public PackedScene Tile { get; set; }
}
