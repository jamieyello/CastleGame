using Castle.Scenes.Entities.Tiles;
using Godot;
using System;

public partial class farm : CharacterBody2D, ITileNode
{
    public Vector2I GridCoords { get; set; }
    public int TeamId { get; set; }
}
