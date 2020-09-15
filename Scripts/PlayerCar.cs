using Godot;
using System;

public class PlayerCar : BaseCar
{

     public override void _Ready()
    {
        base._Ready();
        this.Connect("HitTree", this, nameof(_OnHitTree));
    }

    private void _OnHitTree(PlayerCar car)
    {
        GD.Print("I hit a tree.");
    }

    public override void GetControls(float delta)
    {
        SideSpeed = 0;
        if (Input.IsActionPressed("ui_up"))
        {
            Speed -= Acceleration;
            if (Speed < MaxSpeed)
            {
                Speed = MaxSpeed;
            }
        }
        var speedFromMax = (Speed / MaxSpeed) * 2 < 0.5f ? (Speed / MaxSpeed) * 2 : Speed / MaxSpeed;

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
            Speed = Speed * 0.975f;
        }

        if (Input.IsActionPressed("ui_down"))
        {
            Speed += BrakeDeceleration;
            if (Speed >= 0)
            {
                Speed = 0;
            }
        }

        if (!Input.IsActionPressed("ui_down") && !Input.IsActionPressed("ui_up"))
        {
            if (Speed < 0)
            {
                Speed += Deceleration;
            }
            else if (Speed > 0)
            {
                Speed = 0;
            }
            else
            {
                Speed = 0;
            }
        }
    }
}