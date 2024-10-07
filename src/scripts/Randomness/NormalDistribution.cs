using System.Collections.Generic;
using Godot;

public partial class NormalDistribution : Node2D
{
    private RandomNumberGenerator rng;
    private FastNoiseLite noise;
    private List<Vector2> circles = new();
    private float Y;
    private bool canDraw;
    private float width;
    private float height;

    private float offsetX = 0f;
    private float offsetY = 10000f;

    public override async void _Ready()
    {
        width = GetViewport().GetVisibleRect().Size.X / 2;
        height = GetViewport().GetVisibleRect().Size.Y / 2;
        rng = new RandomNumberGenerator();
        noise = new FastNoiseLite
        {
            NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin
        };

        Y = GetViewport().GetVisibleRect().Size.Y / 2;

        await ToSignal(GetTree().CreateTimer(2.0f), "timeout");
        canDraw = true;

    }

    public override void _PhysicsProcess(double delta)
    {
        if (!canDraw)
        {
            return;
        }

        // Perlin();
        NormalDistro();
        QueueRedraw();
    }

    public override void _Draw()
    {
        foreach (var circle in circles)
        {
            DrawCircle(circle, 60, new Color(Colors.Aquamarine, .025f));
        }
    }

    private void NormalDistro()
    {
        var x = rng.Randfn(360, 130);
        // var x = rng.Randfn(360, 130);
        // var y = rng.Randfn(640, 210);
        circles.Add(new Vector2(x, height));
    }

    private void Perlin()
    {
        var nX = noise.GetNoise1D(offsetX);
        var nY = noise.GetNoise1D(offsetY);
        var x = Mathf.Remap(nX, 0, 1, 0, width) * 3;
        var y = Mathf.Remap(nY, 0, 1, 0, height) * 3;

        circles.Add(new Vector2(x + width, y + height));

        offsetX += 0.15f;
        offsetY += 0.15f;
    }

    private void OnTimerTimeout()
    {
        canDraw = true;
    }
}
