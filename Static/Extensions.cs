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
        if (value.Y > cap.Y) value.Y = cap.Y;
        if (value.X < -cap.X) value.X = -cap.X;
        if (value.Y < -cap.Y) value.Y = -cap.Y;
        return value;
    }
}

