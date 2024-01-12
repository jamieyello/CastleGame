using Castle.Scenes.Entities.Creatures;
using Godot;
using System;

public partial class Archer : Creature
{
    public override void _Ready()
    {
        CreatureReady();
        Behavior = new(this, new() { {
                CreatureBehaviorMode.Type.Idle,
                new() {
                    Animation = "default",
                    GetAdditionalDirection = (d) => (Vector2)Utility.GetRandomMovementInt(),
                    ProcessCreatureInVision = ProcessCreatureInVision,
                    GetFlip = null,
                }
            }
        });
    }

    bool ProcessCreatureInVision(Creature creature)
    {
        if (IsEnemy(this, creature))
        {
            //if (creature.IsBeast)
            //{
            //    Behavior.ChangeMode(CreatureBehaviorMode.Type.Scaring, creature);
            //    creature.Scare(this);
            //    return true;
            //}
        }
        return false;
    }

    Vector2 GetShakeTowards(double delta)
    {
        var velocity = Utility.Towards(GlobalPosition, Behavior.Focus.AsNode().GlobalPosition) * 50f;
        velocity += Utility.GetRandomMovementInt() * 4;
        return velocity;
    }

    public override void _Draw()
    {
        CreatureDraw();
    }

    public override void _PhysicsProcess(double delta)
    {
        CreatureProcessPhysics(delta);
        Behavior.ProcessPhysics(delta);
    }

    void _BodyEnteredVision(Node2D node) => Behavior.BodyEnteredVision(node);
    void _BodyExitedVision(Node2D node) => Behavior.BodyExitedVision(node);
}

