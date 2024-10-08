using Godot;

namespace Game.Bee;
public partial class Bee : Node2D
{
    [Export] private BeeScene scene;
    private FastNoiseLite noise;

    private float height;
    private float width;

    private float offsetX = 0;
    private float offsetY = 1000000;
    private float time = 0.0f;
    private float modifier = 1.0f;
    private Vector2 nextPosition = new(0, 0);
    private Vector2 velocity = new(0, 0);
    private Vector2 acceleration = new(0, 0);

    public override void _Ready()
    {
        height = GetViewportRect().Size.Y;
        width = GetViewportRect().Size.X;

        noise = new FastNoiseLite
        {
            // NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin
            NoiseType = FastNoiseLite.NoiseTypeEnum.SimplexSmooth,
            Frequency = 0.01f
        };
    }

    public override void _PhysicsProcess(double delta)
    {

        if (scene.isDragging)
        {
            modifier = .0015f;
            GetMouse();
        }
        else if (!scene.isDragging)
        {
            modifier = .0004f;
            Perlin();
        }

        time += (float)delta * modifier;
        Position = Position.Lerp(nextPosition, time);
    }

    private void GetMouse()
    {
        var mousePos = GetLocalMousePosition();
        mousePos.Normalized();
        mousePos *= new Vector2(0.2f, 0.2f);
        acceleration = mousePos;
        velocity = new Vector2(Mathf.Clamp(velocity.X + acceleration.X, -5, 5), Mathf.Clamp(velocity.Y + acceleration.Y, -10, 10));
        nextPosition = new Vector2(nextPosition.X + velocity.X, nextPosition.Y + velocity.Y);
    }

    private void Perlin()
    {
        var nX = noise.GetNoise1D(offsetX);
        var nY = noise.GetNoise1D(offsetY);
        var x = Mathf.Remap(nX, 0, 1, 0, width);
        var y = Mathf.Remap(nY, 0, 1, 0, height);

        var mousePos = GetLocalMousePosition();

        nextPosition = new Vector2(x + width / 2, y + height / 2);

        offsetX += 0.1f;
        offsetY += 0.1f;
    }
}
