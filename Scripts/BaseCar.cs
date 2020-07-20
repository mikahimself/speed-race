using Godot;
using System;

public class BaseCar : KinematicBody2D
{
    [Export]
    public float MaxSpeed = -650.0f;
    [Export]
    public float Acceleration = 1.0f;
    public float Deceleration = 5.0f;
    private float _speed = 0.0f;
    
    private Vector2 _velocity;

    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void GetControls(float delta)
    {
        if (Input.IsActionPressed("ui_up"))
        {
            _speed -= Acceleration;
            if (_speed < MaxSpeed)
            {
                _speed = MaxSpeed;
            }
        }
        else
        {
            _speed += Deceleration;
            if (_speed > 0)
            {
                _speed = 0;
            }
        }
        GD.Print(_speed);
    }

    public override void _PhysicsProcess(float delta)
    {
        GetControls(delta);
        _velocity = new Vector2(GlobalPosition.x, _speed);
        MoveAndSlide(_velocity);
    }
}
