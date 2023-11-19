using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Extensions
{
    public static Vector2 AbsCapVector2(this Vector2 value, Vector2 cap)
    {
        if (value.X > cap.X) value.X = cap.X;
        if (value.X < -cap.X) value.X = -cap.X;

        if (value.Y > cap.Y) value.Y = cap.Y;
        if (value.Y < -cap.Y) value.Y = -cap.Y;
        return value;
    }

    public static bool AbsBothAreLessThan(this Vector2 value, float cutoff) =>
        (Mathf.Abs(value.X) < cutoff) && (Mathf.Abs(value.Y) < cutoff);
    
    /// <summary> Returns true if X or Y is negative. </summary>
    public static bool IsNegative(this Vector2 v) =>
        v.X < 0f || v.Y < 0f;

}

