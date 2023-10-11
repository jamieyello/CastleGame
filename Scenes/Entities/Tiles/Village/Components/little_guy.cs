using Godot;
using System;

public partial class little_guy : CharacterBody2D
{
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
		AbsCapVector2(ref velocity, SpeedCap);

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

	static void AbsCapVector2(ref Vector2 value, Vector2 cap)
	{
		if (value.X > cap.X) value.X = cap.X;
		if (value.Y > cap.Y) value.Y = cap.Y;
        if (value.X < -cap.X) value.X = -cap.X;
        if (value.Y < -cap.Y) value.Y = -cap.Y;
    }
}
