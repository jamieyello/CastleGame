using Castle.Static;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Castle.Scenes.Entities.Creatures
{
    // All Creatures must;
    // - Be a CharacterBody2D
    // - Call CreatureReady, CreatureDraw, and CreatureProcessPhysics
    // - Have an AnimatedSprite2D with a ShaderMaterial
    public abstract partial class Creature : CharacterBody2D
    {
        public enum AttackResult
        {
            damaged,
            killed,
            blocked,
            missed,
        }

        [Export]
        public CreatureStats Stats { get; set; }
        public CreatureBehavior Behavior { get; protected set; }
        public int TeamId { get; set; }
        public virtual bool IsBeast { get; }
        public bool Highlighted => GlobalData.Player.Runtime.Selection.IsSelected(this);

        protected bool? prev_highlighted { get; set; }

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
            HandleHighlight();
        }

        void HandleHighlight() {
            if (prev_highlighted.HasValue && prev_highlighted == Highlighted) return;
            if (Highlighted) {
                GetSprite().Material = new ShaderMaterial();
                Shader = HighlightShader;
                AsNode().QueueRedraw();
            }
            else {
                Shader = null;
                AsNode().QueueRedraw();
            }
        }
        #endregion

        public void Scare(Creature scarer) {
            Behavior.ChangeMode(CreatureBehaviorMode.Type.Scared, scarer);
        }

        public void Attack(Creature attacker, int damage, out AttackResult result) {
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

        public void Kill(Creature killer) {
            AsNode().QueueFree();
        }

        public void MoveTo(Vector2 position)
        {
            Behavior.ChangeMode(CreatureBehaviorMode.Type.Navigating);
            Behavior.SetValues(position);
        }

        protected static bool IsEnemy(Creature you, Creature other) {
            return you.TeamId != other.TeamId;
        }

        public CharacterBody2D AsNode() => (CharacterBody2D)this;

        private AnimatedSprite2D GetSprite() => AsNode().GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }
}
