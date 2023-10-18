using Castle.Scenes.Entities.Creatures;
using Godot;
using System;
using System.Collections.Generic;

public partial class Wolf : CharacterBody2D, ICreature
{
    public int TeamId { get; set; } = 1;

    [Export]
    public CreatureStats Stats { get; private set; } = new();

    CreatureBehavior State { get; set; }

    public bool IsBeast => true;

    public override void _Ready()
    {
        State = new CreatureBehavior(this, new() {
            { 
                CreatureBehaviorMode.Mode.Idle, 
                new() { 
                    Animation = "default", 
                    GetTime = () =>
                        1f + Random.Shared.NextSingle() * 3f,
                    NextMode = CreatureBehaviorMode.Mode.Wandering,
                    Halt = false,
                }
            },
            {
                CreatureBehaviorMode.Mode.Wandering, 
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
                CreatureBehaviorMode.Mode.Scared, 
                new() {
                    Animation = "run_fast",
                    GetDirection = () => 
                        Utility.Towards(State.Focus.AsNode().GlobalPosition, GlobalPosition) * 200f,
                    GetTime = () =>
                        1f + Random.Shared.NextSingle() * 2f,
                    Distracted = true,
                }
            }
        });
    }

    public override void _PhysicsProcess(double delta) {
        State.ProcessPhysics(delta);
	}

    public void Scare(ICreature scarer) {
        State.ChangeMode(CreatureBehaviorMode.Mode.Scared, scarer);
    }

    public void Attack(ICreature attacker, int damage, out ICreature.AttackResult result)
    {
        Stats.Hp -= damage;
        if (Stats.Hp == 0)
        {
            Kill(attacker);
            result = ICreature.AttackResult.killed;
        }
        else
        {
            Scare(attacker);
            result = ICreature.AttackResult.damaged;
        }
    }

    public void Kill(ICreature killer)
    {
        QueueFree();
    }

    void _BodyEnteredVision(Node2D node) => State.BodyEnteredVision(node);
    void _BodyExitedVision(Node2D node) => State.BodyExitedVision(node);
    void _BodyEnteredAttackRange(Node2D node) => State.BodyEnteredAttackRange(node);
    void _BodyExitedAttackRange(Node2D node) => State.BodyExitedAttackRange(node);
}
