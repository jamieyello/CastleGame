using Castle.Scenes.Entities.Creatures;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CreatureBehaviorMode
{
    public enum Mode
    {
        Idle,
        Wandering,
        Hunting,
        Scared,
        Eating
    }

    /// <summary> The animation to play during this state. </summary>
    public string Animation { get; init; }

    /// <summary> If set to null, the state will not end automatically. </summary>
    public Func<float> GetTime { get; init; }

    /// <summary> The next <see cref="Mode"/> to transition to if <see cref="GetTime"/> is set. <see cref="Mode.Idle"/> by default. </summary>
    public Mode NextMode { get; init; }

    /// <summary> Direction to apply every frame. Called once at the start. </summary>
    public Func<Vector2> GetDirection { get; init; }

    /// <summary> Direction to apply every frame. Called every frame. </summary>
    public Func<double, Vector2> GetAdditionalDirection { get; init; }

    /// <summary> Whether the sprite should flip horizontally or not. By default looks at the velocity of the creature. (<see cref="DefaultGetFlip"/>) </summary>
    /// <remarks> (ICreature creature, double delta) </remarks>
    public Func<ICreature, double, bool?> GetFlip { get; init; } = DefaultGetFlip;

    /// <summary> If true, this creature will not commit to any actions until this mode is through. </summary>
    public bool Distracted { get; init; }

    /// <summary> If true, this creature will actively stop moving. </summary>
    public bool Halt { get; init; }

    /// <summary> Multiply the existing velocity by this value per update. </summary>
    public float Friction = 0f;

    /// <summary> Called for each creature in this creature's vision. </summary>
    public Action<ICreature> ProcessCreatureInVision { get; init; }

    /// <summary> Called for every creature in this creature's attack range. </summary>
    public Action<ICreature> ProcessCreatureInAttackRange { get; init; }

    /// <summary> Called every frame. </summary>
    public Action<double> Process { get; init; }

    public static readonly Func<ICreature, double, bool?> DefaultGetFlip =
        (ICreature creature, double delta) =>
            ((CharacterBody2D)creature).Velocity.X < 0f ? false :
            ((CharacterBody2D)creature).Velocity.X > 0f ? true : null;
}