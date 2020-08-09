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
    private float _speed = 0.0f;
    
    private Vector2 _velocity;
    private int[] offTrackTiles = {
        3, 4, 5, 6, 7, 8, 9, 10, 17, 18, 20, 21, 22, 24, 29, 31, 34, 38, 39, 43, 44, 47,
        50, 53, 56, 59, 62, 65, 74, 75, 82, 83
    };

    private int[] sidelineTiles = {

    };

    TileMap map;
    public override void _Ready()
    {
        map = (TileMap)GetParent().GetNode("TileMap");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void GetControls(float delta)
    {
        SideSpeed = 0;
        if (Input.IsActionPressed("ui_up"))
        {
            _speed -= Acceleration;
            if (_speed < MaxSpeed)
            {
                _speed = MaxSpeed;
            }
        }
        if (Input.IsActionPressed("ui_left"))
        {
            SideSpeed = -MaxSideSpeed;
        }
        if (Input.IsActionPressed("ui_right"))
        {
            SideSpeed = MaxSideSpeed;
        }
        var pos = (map.WorldToMap(GlobalPosition + new Vector2(0, 0)) / 4);
        var mapid = map.GetCellv(pos);
        if (_CheckOffroadTile(mapid))
        {
            SideSpeed = SideSpeed / 2;
            _speed = _speed * 0.975f;
        }

        if (Input.IsActionPressed("ui_down"))
        {
            _speed += Acceleration;
            if (_speed > -MaxSpeed)
            {
                _speed = -MaxSpeed;
            }
        }

        if (!Input.IsActionPressed("ui_down") && !Input.IsActionPressed("ui_up"))
        {
            if (_speed < -5) {
                _speed += Deceleration;
            } else if (_speed > 5){
                _speed -= Deceleration;
            } else {
                _speed = 0;
            }
        }

        //GD.Print(_speed);

    }

    public override void _PhysicsProcess(float delta)
    {
        GetControls(delta);
        _velocity = new Vector2(SideSpeed, _speed);
        MoveAndSlide(_velocity);
    }

    private bool _CheckOffroadTile(int tileID)
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
