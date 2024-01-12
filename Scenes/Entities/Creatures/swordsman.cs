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

    // MAINTENANCE SHIT START
    private NavigationAgent2D _navigationAgent;

    private float _movementSpeed = 200.0f;

    private Vector2 _movementTargetPosition = new();

    public Vector2 MovementTarget
    {
        get { return _navigationAgent.TargetPosition; }
        set { _navigationAgent.TargetPosition = value; }
    }
    // MAINTENANCE SHIT END

    public override void _Ready() {
        // MAINTENANCE SHIT START
        _navigationAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");

        // These values need to be adjusted for the actor's speed
        // and the navigation layout.
        _navigationAgent.PathDesiredDistance = 4.0f;
        _navigationAgent.TargetDesiredDistance = 4.0f;

        // Make sure to not await during _Ready.
        Callable.From(ActorSetup).CallDeferred();
        // MAINTENANCE SHIT END

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

    // MAINTENANCE SHIT START
    private async void ActorSetup()
    {
        // Wait for the first physics frame so the NavigationServer can sync.
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);

        // Now that the navigation map is no longer empty, set the movement target.
        MovementTarget = _movementTargetPosition;
    }
    // MAINTENANCE SHIT END

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
        _movementTargetPosition = pointOfInterest;
    }

    public override void _Draw() {
        ((ICreature)this).CreatureDraw();
    }

    public override void _PhysicsProcess(double delta) {
        ((ICreature)this).CreatureProcessPhysics(delta);
        Behavior.ProcessPhysics(delta);

        // MAINTENANCE SHIT START
        if (_navigationAgent.IsNavigationFinished())
        {
            return;
        }

        Vector2 currentAgentPosition = GlobalTransform.Origin;
        Vector2 nextPathPosition = _navigationAgent.GetNextPathPosition();

        Velocity = currentAgentPosition.DirectionTo(nextPathPosition) * _movementSpeed;
        MoveAndSlide();
        // MAINTENANCE SHIT END
    }

    void _BodyEnteredVision(Node2D node) => Behavior.BodyEnteredVision(node);
    void _BodyExitedVision(Node2D node) => Behavior.BodyExitedVision(node);
}
