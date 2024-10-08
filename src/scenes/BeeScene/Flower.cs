using Godot;

public partial class Flower : Node2D
{
    [Export] private BeeScene scene;
    private Vector2 dragOffset = Vector2.Zero;
    private Vector2 mousePos = Vector2.Zero;

    public override void _Input(InputEvent input)
    {
        if (input is InputEventMouseButton eventMouseButton)
        {
            mousePos = eventMouseButton.Position;
        }

        if (input.IsActionPressed("left_click"))
        {
            if (GetGlobalRect().HasPoint(mousePos))
            {
                scene.isDragging = true;
                dragOffset = Position - mousePos;
            }

        }
        else if (input.IsActionReleased("left_click"))
        {
            scene.isDragging = false;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (scene.isDragging)
        {
            Position = GetViewport().GetMousePosition() + dragOffset;
        }
    }

    private Rect2 GetGlobalRect()
    {
        // Assuming your node is a simple rectangle, adjust this to your shape
        return new Rect2(Position - new Vector2(50, 50), new Vector2(100, 100)); // Example size: 100x100
    }
}
