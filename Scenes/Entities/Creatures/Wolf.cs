using Castle.Scenes.Entities.Creatures;
using Godot;
using System;
using System.Collections.Generic;

public partial class Wolf : CharacterBody2D, ICreature
{
    public int TeamId { get; set; } = 1;

    [Export]
    public CreatureStats Stats { get; private set; } = new();
    public CreatureBehavior Behavior { get; set; }
    public bool IsBeast => true;

    public override void _Ready()
    {
        Behavior = new CreatureBehavior(this, new() {
            { 
                CreatureBehaviorMode.Type.Idle, 
                new() { 
                    Animation = "default", 
                    GetTime = () =>
                        1f + Random.Shared.NextSingle() * 3f,
                    NextMode = CreatureBehaviorMode.Type.Wandering,
                    Halt = true,
                }
            },
            {
                CreatureBehaviorMode.Type.Wandering, 
                new() {
                    Animation = "run",
                    GetDirection = () =>
                        new Vector2(Random.Shared.NextSingle() - 0.5f,
                            Random.Shared.NextSingle() - 0.5f) * 50f,
                    GetTime = () =>
                        0.1f + Random.Shared.NextSingle() * 2f,
                }
            },
            {
                CreatureBehaviorMode.Type.Scared, 
                new() {
                    Animation = "run_fast",
                    GetDirection = () => 
                        Utility.Towards(Behavior.Focus.AsNode().GlobalPosition, GlobalPosition) * 200f,
                    GetTime = () =>
                        1f + Random.Shared.NextSingle() * 2f,
                    Distracted = true,
                }
            }
        });
    }

    public override void _PhysicsProcess(double delta) {
        Behavior.ProcessPhysics(delta);
	}

    void _BodyEnteredVision(Node2D node) => Behavior.BodyEnteredVision(node);
    void _BodyExitedVision(Node2D node) => Behavior.BodyExitedVision(node);
    void _BodyEnteredAttackRange(Node2D node) => Behavior.BodyEnteredAttackRange(node);
    void _BodyExitedAttackRange(Node2D node) => Behavior.BodyExitedAttackRange(node);
}
