using Castle.Scenes.Entities.Creatures;
using Godot;
using System;

public partial class little_guy : Creature
{
    [Export]
	public float MoveDistance = 30f;

	/// <summary> 0f = will not move, 1f = will move every frame. </summary>
	[Export]
	public float MoveRate = 1f;

	[Export]
	public Vector2 SpeedCap = new(1000f, 1000f);

    public override void _Ready() {
        CreatureReady();
        Behavior = new(this, new() { {
				CreatureBehaviorMode.Type.Idle, 
				new() { 
					Animation = "default", 
					ProcessPhysics = (delta, velocity) => {
						velocity += (Vector2)Utility.GetRandomMovementInt(0.2f) * 1f;
						return velocity;//.AbsCapVector2(SpeedCap);
                    },
					Friction = 0.99f,
					ProcessCreatureInVision = ProcessCreatureInVision,
				}
			}, {
				CreatureBehaviorMode.Type.Scared, 
				new() {
					Animation = "default",
					ProcessPhysics = (delta, velocity) => {
                        velocity += (Vector2)Utility.GetRandomMovementInt(0.9f) * 10f;
						return velocity;//.AbsCapVector2(SpeedCap);
                    }, 
					Friction = .99f,
					GetTime = () => Random.Shared.NextSingle() * 5 + 5,
				}
			}
		});
    }

    bool ProcessCreatureInVision(Creature creature) {
        if (IsEnemy(this, creature)) {
			Scare(creature);
			return true;
        }
        return false;
    }

    public override void _Draw() {
        CreatureDraw();
    }

    public override void _PhysicsProcess(double delta) {
        CreatureProcessPhysics(delta);
        Behavior.ProcessPhysics(delta);
    }

    void _BodyEnteredVision(Node2D node) => Behavior.BodyEnteredVision(node);
    void _BodyExitedVision(Node2D node) => Behavior.BodyExitedVision(node);
}
