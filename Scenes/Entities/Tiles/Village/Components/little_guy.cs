using Castle.Scenes.Entities.Creatures;
using Godot;
using System;

public partial class little_guy : CharacterBody2D, ICreature
{
    public int TeamId { get; set; }
	[Export]
    public CreatureStats Stats { get; private set; } = new();
    public CreatureBehavior Behavior { get; private set; }

    public bool IsBeast => false;

    [Export]
	public float MoveDistance = 30f;

	/// <summary> 0f = will not move, 1f = will move every frame. </summary>
	[Export]
	public float MoveRate = 1f;

	[Export]
	public Vector2 SpeedCap = new(300f, 300f);

    public override void _Ready() {
		Behavior = new(this, new() { {
			CreatureBehaviorMode.Type.Idle, 
				new() { 
					Animation = "default", 
					GetAdditionalDirection = (d) => Utility.GetRandomMovementInt(),
					ProcessPhysics = (delta, velocity) => {
						return velocity.AbsCapVector2(SpeedCap);
                    },
				}
			}
		});
    }

    public override void _PhysicsProcess(double delta) {
		Behavior.ProcessPhysics(delta);
	}
}
