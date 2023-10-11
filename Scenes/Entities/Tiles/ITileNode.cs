using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Castle.Scenes.Entities.Tiles
{
    public interface ITileNode
    {
        public Vector2I GridCoords { get; set; }
    }
}
