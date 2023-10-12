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

    enum Mode
    {
        idle,
        shooing,
    }

    Mode mode;

    public float MoveDistance = 30f;
    public float MoveRate = 1f; // 0f = will not move, 1f = will move every frame.

    List<ICreature> creatures_in_vision = new();

    ICreature shooing_creature;
    double? shooing_time;
    AnimatedSprite2D sprite;

    public override void _Ready()
    {
        sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        base._Ready();
    }

    public override void _PhysicsProcess(double delta) {
		Vector2 velocity = Velocity;

        if (mode == Mode.idle)
        {
            sprite.Play("default");
            velocity = GetMovement();

            foreach (var creature in creatures_in_vision)
            {
                if (ICreature.IsEnemy(this, creature))
                {
                    if (creature.IsBeast)
                    {
                        creature.Scare(this);
                        ChangeMode(Mode.shooing);
                        shooing_creature = creature;
                    }
                }
            }
        }
        else if (mode == Mode.shooing)
        {
            sprite.Play("swing_sword");
            velocity = Utility.Towards(GlobalPosition, shooing_creature.AsNode().GlobalPosition) * 50f;
            velocity += GetMovement() * 4f;
            shooing_time -= delta;
            if (shooing_time <= 0) ChangeMode(Mode.idle);
        }

        Velocity = velocity;
		MoveAndSlide();
	}

    void ChangeMode(Mode mode)
    {
        if (this.mode == mode) return;
        shooing_creature = null;
        shooing_time = null;
        this.mode = mode;

        if (mode == Mode.shooing)
        {
            shooing_time = GetShooingTime();
        }
    }

    static float GetShooingTime() =>
        0.2f + Random.Shared.NextSingle() * 1f;

    Vector2 GetMovement() {
        if (Random.Shared.NextSingle() > MoveRate) return Vector2.Zero;
        var r = new Vector2();
        do {
            r.X = (Random.Shared.Next() % 3 - 1) * MoveDistance;
            r.Y = (Random.Shared.Next() % 3 - 1) * MoveDistance;
        } while (r == Vector2.Zero);
        return r;
    }

    public override void _Process(double delta)
    {

        base._Process(delta);
    }

    void _BodyEnteredVision(Node2D node)
    {
        if (node == this) return;
        if (node is ICreature creature) creatures_in_vision.Add(creature);
    }

    void _BodyExitedVision(Node2D node)
    {
        if (node == this) return;
        if (node is ICreature creature) creatures_in_vision.Remove(creature);
    }

    public void Scare(ICreature scarer)
    {
        //throw new NotImplementedException();
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
}
