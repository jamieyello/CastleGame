using Castle.Static;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Castle.Scenes.Entities.Creatures
{
    // All ICreatures must;
    // - Be a CharacterBody2D
    // - Call CreatureReady, CreatureDraw, and CreatureProcessPhysics
    // - Have an AnimatedSprite2D with a ShaderMaterial
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
        public bool Highlighted => GlobalData.Player.Runtime.Selection.IsSelected(this);

        static Shader HighlightShader = GD.Load<Shader>("res://Shaders/highlight.gdshader");

        public Shader Shader {
            get => ((ShaderMaterial)GetSprite().Material).Shader;
            set => ((ShaderMaterial)GetSprite().Material).Shader = value;
        }

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
            if (Highlighted) Shader = HighlightShader;
            else Shader = null;
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
