using Godot;
using System;

public class BaseCar : KinematicBody2D
{
    [Export]
    public float MaxSpeed = -650.0f;
    [Export]
    public float Acceleration = 4.0f;
    public float Deceleration = 10.0f;
    public float SideSpeed = 4.0f;
    public float MaxSideSpeed = 150f;
    protected float _speed = 0.0f;
    
    protected Vector2 _velocity;
    protected int[] offTrackTiles = {
        3, 4, 5, 6, 7, 8, 9, 10, 17, 18, 20, 21, 22, 24, 29, 31, 34, 38, 39, 43, 44, 47,
        50, 53, 56, 59, 62, 65, 74, 75, 82, 83
    };

    protected int[] sidelineTiles = {

    };

    protected TileMap map;

    public virtual void GetControls(float delta)
    {}

    public override void _PhysicsProcess(float delta)
    {
        GetControls(delta);
        _velocity = new Vector2(SideSpeed, _speed);
        MoveAndSlide(_velocity);
    }

    protected bool _CheckOffroadTile(int tileID)
    {
        for (int i = 0; i < offTrackTiles.Length; i++)
        {
            if (offTrackTiles[i] == tileID)
            {
                GD.Print("Offtrack tile: " + tileID);
                return true;
            }
        }
        return false;
    }
}
