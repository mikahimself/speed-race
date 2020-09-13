using Godot;
using System;

public class Cloud : Sprite
{
    public float MoveSpeed { get; set; } = 1;
    public float CloudScale { get; set; } = 1;
    public Vector2 StartPosition;
    int[] cloudScales = new int[] { 2, 3, 4 };
    public float CloudSpeedMin = 0.1f;
    public float CloudSpeedMax = 0.5f;
    int yOffsetMin = 200;
    int yOffsetMax = 1500;

    public void SetCloudPosition(float minX, float maxX, float y, RandomNumberGenerator rng)
    {
        var x = rng.RandfRange(minX, maxX);
        var yNew = rng.RandfRange(y - yOffsetMin, y - yOffsetMax);
        Position = new Vector2(x, yNew);
        StartPosition = new Vector2(x, yNew);
    }

    public void SetScaleAndSpeed(RandomNumberGenerator rng)
    {
        var scaleNo = rng.RandiRange(0, cloudScales.Length - 1);
        Scale = new Vector2(cloudScales[scaleNo], cloudScales[scaleNo]);
        if (rng.RandiRange(0, 1) == 1)
        {
            Scale = new Vector2(Scale.x * -1, Scale.y);
        }
        MoveSpeed = rng.RandfRange(CloudSpeedMin, CloudSpeedMax);
    }

    public void SetCloudFrame(RandomNumberGenerator rng)
    {
        Frame = rng.RandiRange(0, 1);
    }

    public override void _Process(float delta)
    {
        Position += new Vector2(0, MoveSpeed);
    }
}
