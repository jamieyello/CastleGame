using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Castle.Scenes.Entities.Creatures
{
    /// <summary> Handles modular behavior of a creature. </summary>
    public class CreatureBehavior
    {
        readonly Dictionary<CreatureBehaviorMode.Mode, CreatureBehaviorMode> modes;
        readonly List<ICreature> creatures_in_vision = new();
        readonly List<ICreature> creatures_in_attack_range = new();

        readonly ICreature creature;
        readonly CharacterBody2D body;
        readonly AnimatedSprite2D sprite;

        public ICreature Focus { get; private set; }

        CreatureBehaviorMode mode;
        double? time;
        Vector2? direction;

        public CreatureBehavior(ICreature creature, Dictionary<CreatureBehaviorMode.Mode, CreatureBehaviorMode> modes)
        {
            this.creature = creature;
            body = (CharacterBody2D)creature;
            sprite = body.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
            this.modes = new(modes);
            ChangeMode(CreatureBehaviorMode.Mode.Idle);
        }

        public void ProcessPhysics(double delta) {
            var velocity = body.Velocity;

            velocity *= mode.Friction;
            if (direction != null) velocity += direction.Value;
            if (mode.GetAdditionalDirection != null) velocity += mode.GetAdditionalDirection(delta);
            if (mode.Halt) velocity = Vector2.Zero;
            if (mode.ProcessCreatureInVision != null) {
                foreach (var c in creatures_in_vision) mode.ProcessCreatureInVision(c);
            }
            if (mode.ProcessCreatureInAttackRange != null) {
                foreach (var c in creatures_in_attack_range) mode.ProcessCreatureInAttackRange(c);
            }

            mode.Process?.Invoke(delta);

            body.Velocity = velocity;
            body.MoveAndSlide();

            if (mode.GetFlip != null) { 
                var flip = mode.GetFlip(creature, delta);
                if (flip != null) sprite.FlipH = flip.Value;
            }

            if (time != null) {
                time -= delta;
                if (time <= 0) ChangeMode(mode.NextMode);
            }
        }

        public void ChangeMode(CreatureBehaviorMode.Mode mode, ICreature focus = null)
        {
            if (!modes.ContainsKey(mode)) throw new Exception($"No behavior defined for {mode}");
            Focus = focus;
            this.mode = modes[mode];
            time = this.mode.GetTime != null ? this.mode.GetTime() : null;
            direction = this.mode.GetDirection != null ? this.mode.GetDirection() : null;
            sprite.Play(this.mode.Animation);
        }

        public void BodyEnteredVision(Node2D node) {
            if (node == body) return;
            if (node is ICreature creature) creatures_in_vision.Add(creature);
        }

        public void BodyExitedVision(Node2D node) {
            if (node == body) return;
            if (node is ICreature creature) creatures_in_vision.Remove(creature);
        }

        public void BodyEnteredAttackRange(Node2D node) {
            if (node == body) return;
            if (node is ICreature creature) creatures_in_attack_range.Add(creature);
        }

        public void BodyExitedAttackRange(Node2D node) {
            if (node == body) return;
            if (node is ICreature creature) creatures_in_attack_range.Remove(creature);
        }
    }
}
