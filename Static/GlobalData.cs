using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Castle.Static
{
    public static class GlobalData
    {
        public static PlayerGameData Player { get; private set; } = GD.Load<PlayerGameData>("uid://fujdr605jxjx");
    }
}
