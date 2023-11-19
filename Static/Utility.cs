using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class Utility
{
    /// <summary> Returns 1f in the direction of the target. </summary>
    public static Vector2 Towards(Vector2 pov, Vector2 target) => 
        Vector2.Right.Rotated(pov.AngleToPoint(target));

    /// <summary> Returns a random movement in a random direction. (-1, 0, or 1 in both directions.) </summary>
    /// <param name="move_rate"> The odds that a non-zero value is returned. (0f - 1f) </param>
    public static Vector2I GetRandomMovementInt(float move_rate = 1f) {
        if (Random.Shared.NextSingle() > move_rate) return Vector2I.Zero;
        var r = new Vector2I();
        do {
            r.X = (Random.Shared.Next() % 3 - 1);
            r.Y = (Random.Shared.Next() % 3 - 1);
        } while (r == Vector2.Zero);
        return r;
    }

    public static Rect2 GetSelectRect(Vector2 starting_point, Vector2 current_point) {
        var point = starting_point;
        var size = current_point - starting_point;

        if (size.X < 0f) {
            point.X += size.X;
            size.X = MathF.Abs(size.X);
        }

        if (size.Y < 0f) {
            point.Y += size.Y;
            size.Y = MathF.Abs(size.Y);
        }

        return new(point, size);
    }

    // wow
    //public static Rect2 GetSelectRect(Vector2 starting_point, Vector2 current_point) => new(
    //        MathF.Min(starting_point.X, current_point.X),
    //        MathF.Min(starting_point.Y, current_point.Y),
    //        MathF.Max(starting_point.X, current_point.X),
    //        MathF.Max(starting_point.Y, current_point.Y)
    //    );

    //public static Rect2 GetSelectRect(Vector2 starting_point, Vector2 current_point)
    //{
    //    var size = starting_point - current_point;

    //    return new Rect2(
    //        MathF.Min(size.X, 0f),
    //        MathF.Min(size.Y, 0f),
    //        MathF.Abs(size.X),
    //        MathF.Abs(size.Y)

    //        );
    //}
}

