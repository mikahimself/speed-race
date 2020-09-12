using Godot;
using System;

public class BaseCar : KinematicBody2D
{
    [Export]
    public float MaxSpeed = -650.0f;
    [Export]
    public float Acceleration = 4.0f;
    public float Deceleration = 6.0f;
    public float BrakeDeceleration = 15.0f;
    public float SideSpeed = 4.0f;
    public float MaxSideSpeed = 175f;
    public float Speed = 0.0f;
    
    protected Vector2 _velocity;
    protected int[] offTrackTiles = {
        3, 4, 5, 6, 7, 8, 9, 10, 11, 17, 18, 20, 21, 22, 24, 26, 27, 29, 31, 32, 34, 38, 39, 43, 44, 47,
        50, 53, 56, 59, 62, 65, 72, 73, 74, 75, 80, 81, 82, 83
    };

    public int[] onTrackTiles = {
        0, 1, 2, 16, 19, 25, 30, 35, 40, 41, 45, 48, 51, 54, 57, 60, 61, 63, 66, 67, 68, 69, 71,
        76, 78, 79
    };

    protected int[] sidelineTiles = {

    };

    public TileMap Map { get; set; }
    public override void _Ready()
    {}

    public virtual void GetControls(float delta)
    {}

    public override void _PhysicsProcess(float delta)
    {
        GetControls(delta);
        _velocity = new Vector2(SideSpeed, Speed);
        MoveAndSlide(_velocity);
    }

    protected bool _CheckOffroadTile(int tileID)
    {
        for (int i = 0; i < onTrackTiles.Length; i++)
        {
            if (onTrackTiles[i] == tileID)
            {
                return false;
            }
        }
        return true;
    }
}
