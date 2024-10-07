using System.Collections.Generic;
using Godot;

public partial class RandomWalker : Node2D
{
    private float x;
    private float y;
    private RandomNumberGenerator rng;
    private List<Vector2> points = new();
    private bool canDraw;
    private float startingX;
    private float startingY;

    public async override void _Ready()
    {
        x = GetViewport().GetVisibleRect().Size.X / 2;
        y = GetViewport().GetVisibleRect().Size.Y / 2;
        startingX = x;
        startingY = y;
        rng = new RandomNumberGenerator();

        await ToSignal(GetTree().CreateTimer(2.0f), "timeout");
        canDraw = true;
    }

    public override void _Process(double delta)
    {
        if (!canDraw) return;

        GaussianWalk();
        QueueRedraw();
    }

    public override void _Draw()
    {
        var radius = 5;
        var color = new Color(Colors.CadetBlue);
        foreach (var point in points)
        {
            DrawCircle(point, radius, color);
        }
    }

    private void GaussianWalk()
    {
        var xStep = rng.RandfRange(startingX, 130);
        var yStep = rng.RandfRange(startingY, 210);

        x = xStep;
        y = yStep;

        points.Add(new Vector2(x, y));
    }

    private void StepFavorMouseLocation()
    {
        var mousePosition = GetLocalMousePosition();
        var direction_to_mouse = (mousePosition - new Vector2(x, y)).Normalized();

        var step = rng.RandfRange(0, 1);

        if (step < 0.5)
        {
            x += direction_to_mouse.X;
            y += direction_to_mouse.Y;
        }
        else if (step < 0.6)
        {
            x++;
        }
        else if (step < 0.7)
        {
            x--;
        }
        else if (step < 0.8)
        {
            y++;
        }
        else
        {
            y--;
        }

        // if (step < 0.4)
        // {
        //     x++;
        // }
        // else if (step < 0.6)
        // {
        //     x--;
        // }
        // else if (step < 0.8)
        // {
        //     y++;
        // }
        // else
        // {
        //     y--;
        // }
    }

    private void Step()
    {
        var xStep = rng.RandfRange(-1, 1);
        var yStep = rng.RandfRange(-1, 1);

        x += xStep;
        y += yStep;
    }

    private void StepWithBig()
    {
        var rand = rng.RandfRange(0, 1);
        float xStep;
        float yStep;

        if (rand < 0.01f)
        {
            xStep = rng.RandfRange(-100, 100);
            yStep = rng.RandfRange(-100, 100);
        }

        xStep = rng.RandfRange(-1, 1);
        yStep = rng.RandfRange(-1, 1);

        x += xStep * 5;
        y += yStep * 5;

        points.Add(new Vector2(x, y));
    }

    private void RandomWalk()
    {
        var choice = rng.RandiRange(0, 3);

        switch (choice)
        {
            case 0:
                x++;
                break;
            case 1:
                x--;
                break;
            case 2:
                y++;
                break;
            case 3:
                y--;
                break;
            default:
                break;
        }
    }

}
