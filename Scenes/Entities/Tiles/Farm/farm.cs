using Castle.Scenes.Entities.Tiles;
using Castle.Static;
using Godot;
using System;

public partial class Farm : CharacterBody2D, ITileNode
{
    public Vector2I GridCoords { get; set; }
    public int TeamId { get; set; }

    public double TotalGrowthTime { get; set; } = 10f;
    public double CurrentGrowthTime { get; set; } = 0f;

    AnimatedSprite2D sprite;

    public override void _Ready()
    {
        sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        ProcessGrowth(delta);
    }

    void ProcessGrowth(double delta) {
        CurrentGrowthTime += delta;

        if (CurrentGrowthTime < TotalGrowthTime * 0.33f) sprite.Play("default");
        else if (CurrentGrowthTime < TotalGrowthTime * 0.67f) sprite.Play("mid");
        else sprite.Play("grown");
    }
}
