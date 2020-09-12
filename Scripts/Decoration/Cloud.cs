using Godot;
using System;

public class Cloud : Sprite
{
    public float MoveSpeed { get; set; } = 1;
    public float CloudScale { get; set; } = 1;
    public Vector2 StartPosition;
    int[] cloudScales = new int[] { 4, 6, 8};

    public override void _Ready()
    {
        
    }

    public void SetCloudPosition(float minX, float maxX, float y, RandomNumberGenerator rng)
    {
        var x = rng.RandfRange(minX, maxX);
        var yNew = rng.RandfRange(y - 100, y - 500);
        Position = new Vector2(x, yNew);
        StartPosition = new Vector2(x, yNew);
    }

    public void SetScaleAndSpeed(RandomNumberGenerator rng)
    {

        var scaleNo = rng.RandiRange(0, cloudScales.Length - 1);
        Scale = new Vector2(cloudScales[scaleNo], cloudScales[scaleNo]);
        MoveSpeed = rng.RandfRange(1f, 3.0f);
    }

    public override void _Process(float delta)
    {
        Position += new Vector2(0, MoveSpeed);
    }
}
