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
}

