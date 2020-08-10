using Godot;
using System;

public class PlayerCar : BaseCar
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        map = (TileMap)GetParent().GetNode("TileMap");
    }

    public override void GetControls(float delta)
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
            if (_speed < -5)
            {
                _speed += Deceleration;
            }
            else if (_speed > 5)
            {
                _speed -= Deceleration;
            }
            else
            {
                _speed = 0;
            }
        }
        //GD.Print(_speed);
    }

    

}