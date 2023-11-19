using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Castle.Scenes.Entities.Creatures
{
    // I am afraid of making an abstract class for a node.
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
        public bool Highlight { get; set; }

        // These must be called in every ICreature.
        #region Hooks
        public void CreatureReady()
        {

        }

        public void CreatureDraw()
        {

        }

        public void CreatureProcessPhysics(double f)
        {
            var sprite = GetSprite();
            //sprite.Material.S
        }
        #endregion

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

        static bool IsEnemy(ICreature you, ICreature other) {
            return you.TeamId != other.TeamId;
        }

        public CharacterBody2D AsNode() => (CharacterBody2D)this;

        private AnimatedSprite2D GetSprite() => AsNode().GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }
}
