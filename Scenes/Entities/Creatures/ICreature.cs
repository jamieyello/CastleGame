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
        public CreatureBehavior Behavior { get; }
        public int TeamId { get; set; }
        public bool IsBeast { get; }

        public virtual void Scare(ICreature scarer) {
            Behavior.ChangeMode(CreatureBehaviorMode.Type.Scared, scarer);
        }

        public virtual void Attack(ICreature attacker, int damage, out AttackResult result) {
            Stats.Hp -= damage;
            if (Stats.Hp == 0) {
                Kill(attacker);
                result = AttackResult.killed;
            }
            else {
                Scare(attacker);
                result = AttackResult.damaged;
            }
        }

        public virtual void Kill(ICreature killer) {
            AsNode().QueueFree();
        }

        static bool IsEnemy(ICreature you, ICreature other)
        {
            return you.TeamId != other.TeamId;
        }

        public CharacterBody2D AsNode() => (CharacterBody2D)this;
    }
}
