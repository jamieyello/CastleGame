using Castle.Scenes.Entities.Creatures;
using Godot;
using System;
using System.Collections.Generic;

public partial class swordsman : CharacterBody2D, ICreature
{
    public int TeamId { get; set; }
    public bool IsBeast => false;

    [Export]
    public CreatureStats Stats { get; private set; } = new();
    public CreatureBehavior Behavior { get; private set; }

    public override void _Ready() {
        Behavior = new(this, new() { {
                CreatureBehaviorMode.Type.Idle,
                new() {
                    Animation = "default",
                    GetAdditionalDirection = (d) => (Vector2)Utility.GetRandomMovementInt(),
                    ProcessCreatureInVision = ProcessCreatureInVision,
                    GetFlip = null,
                }
            }, {
                CreatureBehaviorMode.Type.shooing, new() { 
                    Animation = "swing_sword",
                    GetAdditionalDirection = GetShakeTowards,
                    GetTime = () => 0.2f + Random.Shared.NextSingle() * 1f,
                }
            }
        });
    }

    void ProcessCreatureInVision(ICreature creature) {
        if (ICreature.IsEnemy(this, creature)) {
            if (creature.IsBeast) {
                Behavior.ChangeMode(CreatureBehaviorMode.Type.shooing, creature);
                creature.Scare(this);
            }
        }
    }

    Vector2 GetShakeTowards(double delta) {
        var velocity = Utility.Towards(GlobalPosition, Behavior.Focus.AsNode().GlobalPosition) * 50f;
        velocity += Utility.GetRandomMovementInt() * 4;
        return velocity;
    }

    public override void _PhysicsProcess(double delta) {
        Behavior.ProcessPhysics(delta);
	}

    public override void _Process(double delta) {
        Behavior.ProcessPhysics(delta);
    }

    void _BodyEnteredVision(Node2D node) => Behavior.BodyEnteredVision(node);
    void _BodyExitedVision(Node2D node) => Behavior.BodyExitedVision(node);
}
