using Godot;
using System;

[GlobalClass]
public partial class PlayerGameData : Resource
{
    [Export]
    public int Id;
    [Export]
    public string Name;
    [Export]
    public double Gold;
    [Export]
    public Color Color;

    public PlayerGameRuntimeData Runtime = new();
}
