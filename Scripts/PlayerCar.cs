using Godot;
using System;

public class PlayerCar : BaseCar
{

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
        var speedFromMax = (_speed / MaxSpeed) * 2 < 0.5f ? (_speed / MaxSpeed) * 2 : _speed / MaxSpeed;

        if (Input.IsActionPressed("ui_left"))
        {
            SideSpeed = -MaxSideSpeed;// * speedFromMax;
        }
        if (Input.IsActionPressed("ui_right"))
        {
            SideSpeed = MaxSideSpeed;// * speedFromMax;
        }
        var pos = (Map.WorldToMap(GlobalPosition) / 4);
        var mapid = Map.GetCellv(pos);
        if (_CheckOffroadTile(mapid))
        {
            SideSpeed = SideSpeed / 2;
            _speed = _speed * 0.975f;
        }

        if (Input.IsActionPressed("ui_down"))
        {
            _speed += BrakeDeceleration;
            if (_speed >= 0)
            {
                _speed = 0;
            }
        }

        if (!Input.IsActionPressed("ui_down") && !Input.IsActionPressed("ui_up"))
        {
            if (_speed < 0)
            {
                _speed += Deceleration;
            }
            else if (_speed > 0)
            {
                _speed -= Deceleration;
            }
            else
            {
                _speed = 0;
            }
        }
    }
}