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
    public bool Highlight { get; set; }
    bool? ICreature.prev_highlighted { get; set; }

    public override void _Ready() {
        ((ICreature)this).CreatureReady();
        Behavior = new(this, new() { {
                CreatureBehaviorMode.Type.Idle,
                new() {
                    Animation = "default",
                    GetAdditionalDirection = (d) => (Vector2)Utility.GetRandomMovementInt(),
                    ProcessCreatureInVision = ProcessCreatureInVision,
                    GetFlip = null,
                }
            }, {
                CreatureBehaviorMode.Type.Scaring, new() { 
                    Animation = "swing_sword",
                    GetAdditionalDirection = GetShakeTowards,
                    GetTime = () => 0.2f + Random.Shared.NextSingle() * 1f,
                }
            }
        });
    }

    bool ProcessCreatureInVision(ICreature creature) {
        if (ICreature.IsEnemy(this, creature)) {
            if (creature.IsBeast) {
                Behavior.ChangeMode(CreatureBehaviorMode.Type.Scaring, creature);
                creature.Scare(this);
                return true;
            }
        }
        return false;
    }

    Vector2 GetShakeTowards(double delta) {
        var velocity = Utility.Towards(GlobalPosition, Behavior.Focus.AsNode().GlobalPosition) * 50f;
        velocity += Utility.GetRandomMovementInt() * 4;
        return velocity;
    }

    public void MoveTo(Vector2 pointOfInterest)
    {
        //_movementTargetPosition = pointOfInterest;
    }

    public override void _Draw() {
        ((ICreature)this).CreatureDraw();
    }

    public override void _PhysicsProcess(double delta) {
        ((ICreature)this).CreatureProcessPhysics(delta);
        Behavior.ProcessPhysics(delta);
    }

    void _BodyEnteredVision(Node2D node) => Behavior.BodyEnteredVision(node);
    void _BodyExitedVision(Node2D node) => Behavior.BodyExitedVision(node);
}
