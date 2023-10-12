using Castle.Scenes.Entities.Creatures;
using Godot;
using System;

public partial class little_guy : CharacterBody2D, ICreature
{
    public int TeamId { get; set; }
	[Export]
    public CreatureStats Stats { get; private set; } = new();

    public bool IsBeast => false;

    [Export]
	public float MoveDistance = 30f;

	/// <summary> 0f = will not move, 1f = will move every frame. </summary>
	[Export]
	public float MoveRate = 1f;

	[Export]
	public Vector2 SpeedCap = new(300f, 300f);

    public override void _PhysicsProcess(double delta)
	{
		var velocity = Velocity;

        velocity += GetMovement();
        velocity = velocity.AbsCapVector2(SpeedCap);

		Velocity = velocity;
		MoveAndSlide();
	}

	Vector2 GetMovement() {
		if (Random.Shared.NextSingle() > MoveRate) return Vector2.Zero;
        var r = new Vector2();
		do {
			r.X = (Random.Shared.Next() % 3 - 1) * MoveDistance;
			r.Y = (Random.Shared.Next() % 3 - 1) * MoveDistance;
        } while (r == Vector2.Zero);
		return r;
	}

    public void Scare(ICreature scarer)
    {
        throw new NotImplementedException();
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
