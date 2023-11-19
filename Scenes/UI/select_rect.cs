using Castle.Scenes.Entities.Creatures;
using Castle.Static;
using Godot;
using System;
using System.Collections.Generic;

public partial class select_rect : Area2D
{
	/// <summary> A list of all contained creatures. Not necessarily the target team. </summary>
	List<ICreature> contained_creatures = new();

	public int TargetTeam;

	RectangleShape2D shape;
	Vector2 start_mouse_p;
	CollisionShape2D c_shape;

    static readonly Color RectColor = new(0f, 0.5f, 1f, 0.5f);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        shape = (RectangleShape2D)GetNode<CollisionShape2D>("CollisionShape2D").Shape;
        c_shape = GetNode<CollisionShape2D>("CollisionShape2D");
		UpdateArea(true);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        var held = Input.IsMouseButtonPressed(MouseButton.Right);

		if (!held)
		{
			QueueFree();
			return;
		}

		UpdateArea();

		QueueRedraw();
		//foreach (var c in contained_creatures) 
    }

	void UpdateArea(bool set_start = false)
	{
        var vp = GetViewport();
        var camera = vp.GetCamera2D();
        var mouse_p = vp.GetMousePosition();
		mouse_p += (camera.Position - camera.GetViewportRect().Size / 2);

		if (set_start)
		{
			start_mouse_p = mouse_p;
		}

		var rect = Utility.GetSelectRect(start_mouse_p, mouse_p);
		Position = rect.Position + rect.Size / 2;
		shape.Size = rect.Size;
    }

    public override void _Draw()
    {
		DrawRect(shape.GetRect(), RectColor);
        //base._Draw();
    }

    void _BodyEntered(Node2D body) {
		if (body is ICreature c) {
			contained_creatures.Add(c);
			if (c.TeamId == TargetTeam) GlobalData.Player.Runtime.Selection.Add(c);
		}
	}

    void _BodyExited(Node2D body) {
		if (body is ICreature c) {
			contained_creatures.Remove(c);
		}
    }
}
