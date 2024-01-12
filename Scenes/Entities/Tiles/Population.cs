using Castle.Scenes.Entities.Creatures;
using Godot;
using System;

public partial class Population : Node2D
{
	/// <summary> The <see cref="ICreature"/> that will represent a unit of population. </summary>
	[Export] public PackedScene Creature { get; set; }

	/// <summary> How many creatures can be spawned to represent the population. </summary>
	[Export] public int MaxCreatureCount { get; set; } = 8;

	/// <summary> How many creature should be spawned on startup. </summary>
	[Export] public int CreatureCount { get; set; } = 1;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ResetSpawns();
	}

	void ResetSpawns() {
		// Remove all existing creatures
		foreach (var c in GetChildren()) {
			if (c.Name == "SpawnPoints") continue;
			c.QueueFree();
		}

		// Add all children needed
        for (int i = 0; i < CreatureCount && i < MaxCreatureCount; i++) {
            var n = Creature.Instantiate<CharacterBody2D>();
			n.Name += i.ToString();
            AddChild(n);
        }
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
