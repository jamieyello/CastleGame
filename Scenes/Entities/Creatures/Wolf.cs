using Castle.Scenes.Entities.Creatures;
using Godot;
using System;
using System.Collections.Generic;

public partial class Wolf : Creature
{
    public override bool IsBeast => true;

    public override void _Ready() {
        TeamId = 1;
        CreatureReady();
        Behavior = new CreatureBehavior(this, new() { { 
                CreatureBehaviorMode.Type.Idle, 
                new() { 
                    Animation = "default", 
                    GetTime = () =>
                        1f + Random.Shared.NextSingle() * 3f,
                    NextMode = CreatureBehaviorMode.Type.Wandering,
                    Halt = true,
                    ProcessCreatureInVision = ProcessCreatureInVision
                }
            }, {
                CreatureBehaviorMode.Type.Wandering, 
                new() {
                    Animation = "run",
                    GetDirection = () =>
                        new Vector2(Random.Shared.NextSingle() - 0.5f,
                            Random.Shared.NextSingle() - 0.5f) * 50f,
                    GetTime = () =>
                        0.1f + Random.Shared.NextSingle() * 2f,
                }
            }, {
                CreatureBehaviorMode.Type.Scared, 
                new() {
                    Animation = "run_fast",
                    GetDirection = () => 
                        Utility.Towards(Behavior.Focus.AsNode().GlobalPosition, GlobalPosition) * 200f,
                    GetTime = () =>
                        1f + Random.Shared.NextSingle() * 2f,
                    Distracted = true,
                }
            }, {
                CreatureBehaviorMode.Type.Hunting, 
                new() {
                    Animation = "run_fast",
                    GetAdditionalDirection = (delta) => Utility.Towards(GlobalPosition, Behavior.Focus.AsNode().GlobalPosition) * 150f,
                    Distracted = true,
                    ProcessCreatureInAttackRange = HuntingProcessCreatureInAttackRange,
                    RequireFocus = true,
                }
            }, {
                CreatureBehaviorMode.Type.MeleeAttack,
                new() {
                    Animation = "default",
                    GetTime = () => 1f,
                    Distracted = true,
                    TriggerStart = ProcessAttack,
                    NextMode = CreatureBehaviorMode.Type.Hunting,
                    RequireFocus = true,
                }
            }, {
                CreatureBehaviorMode.Type.Eating,
                new() {
                    Animation = "eat",
                    GetTime = () => 1f,
                    Distracted = true,
                }
            }
        });
    }

    public override void _Draw() {
        CreatureDraw();
    }

    public override void _PhysicsProcess(double delta) {
        CreatureProcessPhysics(delta);
        Behavior.ProcessPhysics(delta);
    }

    (CreatureBehaviorMode.Type NewMode, Creature NewTarget)? ProcessAttack()
    {
        Behavior.Focus.Attack(this, Stats.Attack, out var result);
        if (result == AttackResult.killed) return (CreatureBehaviorMode.Type.Eating, null);
        return null;
    }

    bool ProcessCreatureInVision(Creature creature) {
        if (IsEnemy(this, creature)) {
            Behavior.ChangeMode(CreatureBehaviorMode.Type.Hunting, creature);
            return true;
        }
        return false;
    }

    bool HuntingProcessCreatureInAttackRange(Creature creature) {
        if (creature != Behavior.Focus) return false;
        Behavior.ChangeMode(CreatureBehaviorMode.Type.MeleeAttack, creature);
        return true;
    }

    void _BodyEnteredVision(Node2D node) => Behavior.BodyEnteredVision(node);
    void _BodyExitedVision(Node2D node) => Behavior.BodyExitedVision(node);
    void _BodyEnteredAttackRange(Node2D node) => Behavior.BodyEnteredAttackRange(node);
    void _BodyExitedAttackRange(Node2D node) => Behavior.BodyExitedAttackRange(node);
}
