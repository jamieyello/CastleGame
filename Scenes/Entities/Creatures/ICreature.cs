using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Castle.Scenes.Entities.Creatures
{
    public interface ICreature
    {
        public enum AttackResult
        {
            damaged,
            killed,
            blocked,
            missed,
        }

        public CreatureStats Stats { get; }
        public int TeamId { get; set; }
        public bool IsBeast { get; }
        public void Scare(ICreature scarer);
        public void Attack(ICreature attacker, int damage, out AttackResult result);
        public void Kill(ICreature killer);

        static bool IsEnemy(ICreature you, ICreature other)
        {
            return you.TeamId != other.TeamId;
        }

        public CharacterBody2D AsNode() => (CharacterBody2D)this;
    }
}
