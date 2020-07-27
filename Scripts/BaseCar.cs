using Godot;
using System;

public class BaseCar : KinematicBody2D
{
    [Export]
    public float MaxSpeed = -650.0f;
    [Export]
    public float Acceleration = 4.0f;
    public float Deceleration = 5.0f;
    public float SideSpeed = 5.0f;
    public float MaxSideSpeed = 150f;
    private float _speed = 0.0f;
    
    private Vector2 _velocity;

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
            _speed = -350;
            /*_speed -= Acceleration;
            if (_speed < MaxSpeed)
            {
                _speed = MaxSpeed;
            }*/
        }
        if (Input.IsActionPressed("ui_left"))
        {
            SideSpeed = -MaxSideSpeed;
        }
        if (Input.IsActionPressed("ui_right"))
        {
            SideSpeed = MaxSideSpeed;
        }
        var pos = (map.WorldToMap(GlobalPosition + new Vector2(-4, 0)) / 4);
        var mapid = map.GetCellv(pos);
        if (mapid == 1)
        {
            SideSpeed = SideSpeed / 2;
            _speed = _speed * 0.975f;
        }

        if (Input.IsActionPressed("ui_down"))
        {
            _speed = 350;
            /*_speed += Acceleration;
            if (_speed > -MaxSpeed)
            {
                _speed = -MaxSpeed;
            }*/
        }

        if (!Input.IsActionPressed("ui_down") && !Input.IsActionPressed("ui_up"))
        {
            _speed = 0;
        }

        //GD.Print(_speed);

    }

    public override void _PhysicsProcess(float delta)
    {
        GetControls(delta);
        _velocity = new Vector2(SideSpeed, _speed);
        MoveAndSlide(_velocity);
    }
}
