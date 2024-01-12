using Castle.Static;
using Godot;
using System;

public partial class Camera : Camera2D
{
	public const float Speed = 8.0f;
	public const float Friction = .80f;
	public const float Cutoff = 0.05f;
    public readonly static Vector2 ZoomSpeed = new(0.05f, 0.05f);

    public const float MinZoom = 0.2f;
    public const float MaxZoom = 2f;

    Vector2? mouse_drag_start; // set when the mouse is held down, set to null when released
    Vector2? pos_drag_start;
    bool dragging; // set to true when mouse has dragged over the defined distance, false when released
    float drag_distance_thresold = 5f; // how far the mouse must move before the screen will start dragging
    bool scroll_disabled = false;

	Vector2 velocity;

    public override void _Process(double delta)
	{
        ButtonScroll();
        DragScroll();

        Confine();
    }

    // Kind of ugly but godot doesn't have anything better for UI?
    /// <summary> Disable scroll until mouse is released. </summary>
    public void CaptureScroll() => scroll_disabled = true;

    void DragScroll()
    {
        var held = Input.IsMouseButtonPressed(MouseButton.Left);
        if (!held) { 
            if (mouse_drag_start != null && !dragging)
            {
                GD.Print("Short click");

                foreach (var selected_creature in GlobalData.Player.Runtime.Selection)
                {
                    selected_creature.MoveTo(mouse_drag_start.Value);
                }
            }

            dragging = false;
            mouse_drag_start = null;
            pos_drag_start = null;
            scroll_disabled = false;
            return;
        }

        if (scroll_disabled) return;

        var mp = GetViewport().GetMousePosition();

        if (held && mouse_drag_start == null) {
            mouse_drag_start = mp;
            pos_drag_start = Position;
        }

        var distance = mp.DistanceTo(mouse_drag_start.Value);
        if (distance >= drag_distance_thresold) dragging = true;

        if (dragging) {
            var offset = mouse_drag_start.Value - mp;
            Position = pos_drag_start.Value + offset / Zoom.X;
        }
    }

    void ButtonScroll()
    {
        var speed = Speed / Zoom.X;

        if (Input.IsActionPressed("ui_left")) Position += new Vector2(-speed, 0f);
        if (Input.IsActionPressed("ui_right")) Position += new Vector2(speed, 0f);
        if (Input.IsActionPressed("ui_up")) Position += new Vector2(0f, -speed);
        if (Input.IsActionPressed("ui_down")) Position += new Vector2(0f, speed);

        if (Input.IsActionJustReleased("ui_zoom_in")) Zoom -= ZoomSpeed;
        if (Input.IsActionJustReleased("ui_zoom_out")) Zoom += ZoomSpeed;
    }

    // For some reason the position of the camera object is not confined, just the view. This confines it.
    void Confine()
    {
        if (Zoom.X < MinZoom) Zoom = new(MinZoom, MinZoom);
        if (Zoom.X > MaxZoom) Zoom = new(MaxZoom, MaxZoom);

        var view_s = this.GetViewportRect().Size / Zoom;

        if (Position.X < LimitLeft + view_s.X / 2) {
            Position = Position with { X = LimitLeft + view_s.X / 2 };
            velocity.X = 0f;
        }
        if (Position.X > LimitRight - view_s.X / 2) {
            Position = Position with { X = LimitRight - view_s.X / 2 };
            velocity.X = 0f;
        }
        if (Position.Y < LimitTop + view_s.Y / 2) {
            Position = Position with { Y = LimitTop + view_s.Y / 2 };
            velocity.Y = 0f;
        }
        if (Position.Y > LimitBottom - view_s.Y / 2) {
            Position = Position with { Y = LimitBottom - view_s.Y / 2 };
            velocity.Y = 0f;
        }
    }
}
