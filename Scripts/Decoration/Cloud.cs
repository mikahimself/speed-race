using Godot;
using System;

public class Cloud : Sprite
{
    public float MoveSpeed { get; set; } = 1;
    public float CloudScale { get; set; } = 1;

    public override void _Ready()
    {
        MoveSpeed = (float)GD.RandRange(0.1, 1.0);
        CloudScale = (float)GD.RandRange(1.0, 2.0);
        
        Scale = new Vector2(CloudScale, CloudScale);
    }

    public void SetCloudPosition(float minX, float maxX, float y)
    {
        var x = (float)GD.RandRange(minX, maxX);
        Position = new Vector2(x, y);
    }

    public override void _Process(float delta)
    {
        Position += new Vector2(0, MoveSpeed);
    }
}
