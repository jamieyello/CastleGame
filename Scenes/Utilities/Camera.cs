using Godot;
using System;

public partial class Camera : Camera2D
{
	public const float Speed = 5.0f;
	public const float Friction = .80f;
	public const float Cutoff = 0.05f;
    public readonly static Vector2 ZoomSpeed = new(-0.05f, -0.05f);

    public const float MinZoom = 0.5f;
    public const float MaxZoom = 2f;

	Vector2 velocity;

    public override void _Process(double delta)
	{
        if (Input.IsActionPressed("ui_left")) Position += new Vector2(-Speed, 0f);
        if (Input.IsActionPressed("ui_right")) Position += new Vector2(Speed, 0f);
        if (Input.IsActionPressed("ui_up")) Position += new Vector2(0f, -Speed);
        if (Input.IsActionPressed("ui_down")) Position += new Vector2(0f, Speed);

        if (Input.IsActionJustReleased("ui_zoom_in")) Zoom += ZoomSpeed;
        if (Input.IsActionJustReleased("ui_zoom_out")) Zoom -= ZoomSpeed;

        if (Zoom.X < MinZoom) Zoom = new(MinZoom, MinZoom);
        if (Zoom.X > MaxZoom) Zoom = new(MaxZoom, MaxZoom);
    }
}
