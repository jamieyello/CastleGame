using Castle.Scenes.Entities.Creatures;
using Godot;
using System;
using System.Collections.Generic;

public partial class Wolf : CharacterBody2D, ICreature
{
	public enum Mode
	{
		idle,
		wandering,
		hunting,
        scared,
        eating
	}

    public int TeamId { get; set; } = 1;
    [Export]
    public CreatureStats Stats { get; private set; } = new();

    public bool IsBeast => true;

	Mode mode;
    List<ICreature> creatures_in_vision = new();
    ICreature hunting_target;

    List<ICreature> creatures_in_attack_range = new();

    AnimatedSprite2D sprite;

    float wandering_speed = 50f;
	Vector2? wandering_direction;
	double? wandering_time; // how long to wander (counts down)

    double? idle_time; // how long to be idle before wandering again (counts down)

    float scared_speed = 200f;
    Vector2? scared_direction;
    double? scared_time;

    float hunting_speed = 200f;

    double? eating_time;
    public override void _Ready()
    {
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		mode = Mode.idle;
		idle_time = GetIdleTime();
    }

    public override void _PhysicsProcess(double delta) {
		Vector2 velocity = Velocity;

        if (mode == Mode.idle) {
            sprite.Play("default");
            velocity = Vector2.Zero;
            idle_time -= delta;
            if (idle_time <= 0f) ChangeMode(Mode.wandering);
        }
        else if (mode == Mode.wandering) {
            sprite.Play("run");
            velocity = wandering_direction.Value * wandering_speed;
            wandering_time -= delta;
            if (wandering_time <= 0f) ChangeMode(Mode.idle);
        }
        else if (mode == Mode.hunting) {
            sprite.Play("run_fast");
            velocity = Utility.Towards(GlobalPosition, hunting_target.AsNode().GlobalPosition) * hunting_speed;
        }
        else if (mode == Mode.eating)
        {
            sprite.Play("eat");
            velocity = Vector2.Zero;
            eating_time -= delta;
            if (eating_time <= 0f) ChangeMode(Mode.idle);
        }
        else if (mode == Mode.scared) {
            sprite.Play("run_fast");
            velocity = scared_direction.Value * scared_speed;
            scared_time -= delta;
            if (scared_time <= 0f) ChangeMode(Mode.idle);
        }
        if (!IsDistracted())
        {
            foreach (var creature in creatures_in_attack_range) {
                if (ICreature.IsEnemy(this, creature))
                {
                    creature.Attack(this, Stats.Attack, out var res);
                }
            }
        }

        if (velocity.X < 0f) sprite.FlipH = false;
        if (velocity.X > 0f) sprite.FlipH = true;

        Velocity = velocity;
		MoveAndSlide();
	}

    void ChangeMode(Mode mode)
    {
        this.mode = mode;
        idle_time = null;
        wandering_time = null;
        wandering_direction = null;
        scared_direction = null;
        scared_time = null;
        eating_time = null;

        if (mode == Mode.wandering)
        {
            wandering_direction = GetWanderingDirection();
            wandering_time = GetWanderingTime();
        }
        else if (mode == Mode.idle)
        {
            idle_time = GetIdleTime();
        }
        else if (mode == Mode.hunting)
        {

        }
        else if (mode == Mode.eating)
        {
            eating_time = GetEatingTime();
        }
        else if (mode == Mode.scared)
        {
            scared_time = GetScaredTime();
        }

    }

    public bool IsDistracted() => 
        mode == Mode.eating || 
        mode == Mode.scared ||
        mode == Mode.hunting;

	static Vector2 GetWanderingDirection() => 
		new(Random.Shared.NextSingle() - 0.5f, 
			Random.Shared.NextSingle() - 0.5f);

    static float GetIdleTime() =>
		1f + Random.Shared.NextSingle() * 3f;

    static float GetWanderingTime() =>
	    0.1f + Random.Shared.NextSingle() * 2f;

    static float GetScaredTime() =>
        1f + Random.Shared.NextSingle() * 2f;

    static float GetEatingTime() =>
        0.3f + Random.Shared.NextSingle() * 1f;

    public void Scare(ICreature scarer)
    {
        ChangeMode(Mode.scared);
        scared_direction = Utility.Towards(scarer.AsNode().GlobalPosition, GlobalPosition);
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

    void _BodyEnteredAttackRange(Node2D node)
    {
        if (node == this) return;
        if (node is ICreature creature) creatures_in_attack_range.Add(creature);
    }

    void _BodyExitedAttackRange(Node2D node)
    {
        if (node == this) return;
        if (node is ICreature creature) creatures_in_attack_range.Remove(creature);
    }
}
