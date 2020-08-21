using Godot;
using System;

public class AICar : BaseCar
{
    public enum Direction {
        FORWARD,
        LEFT,
        RIGHT
    }

    [Export]
    public Line2D line;
    public Direction AiDirection = Direction.FORWARD;

    public override void _Ready()
    {
        base._Ready();
        line = (Line2D)GetNode("Line2D");
        line.Points = new Vector2[] { GlobalPosition, GlobalPosition + new Vector2(0, -320)};
    }

    public override void GetControls(float delta)
    {
        switch (AiDirection)
        {
            case Direction.FORWARD:
                SteerForward();
                break;
            case Direction.LEFT:
                SteerSideways(-1);
                break;
            case Direction.RIGHT:
                SteerSideways(1);
                break;
            default:
                SteerForward();
                break;
        }
    }

    private void SteerForward()
    {
        SideSpeed = 0;
        var forwardPos = (map.WorldToMap(GlobalPosition + new Vector2(0, -320)) / 4);
        var forwardPosId = map.GetCellv(forwardPos);
        if (_CheckOffroadTile(forwardPosId))
        {
            line.DefaultColor = new Color(1, 0, 0, 1);
            SetTurnDirection();
            return;
        }
        else
        {
            line.DefaultColor = new Color(0, 0, 1, 1);
        }
        _speed = MaxSpeed + 450;
    }

    private void ScanForward()
    {
        // Tsekkaas ennen eteen ajamista miten kaukana ollaan laidasta.
        var forwardPos = (map.WorldToMap(GlobalPosition + new Vector2(0, -320)) / 4);
        var forwardPosId = map.GetCellv(forwardPos);
        if (!_CheckOffroadTile(forwardPosId))
        {
            AiDirection = Direction.FORWARD;
            GD.Print("Go forward");
        }
        
    }

    private void SteerSideways(int direction)
    {
        SideSpeed = MaxSideSpeed * direction;
        _speed = MaxSpeed + 400;

        var left = ScanSide(-1);
        var right = ScanSide(1);

        if (AiDirection == Direction.LEFT )
        {
            if (left < right || right > 5) {
                ScanForward();
            }
        }
        if (AiDirection == Direction.RIGHT)
        {
            if (left > right || left > 5) {
                ScanForward();
            }
        }
  
//        ScanForward();
    }

    private int ScanSide(int direction)
    {
        int sidePoints = 0;

        for (int i = 1; i <= 10; i++)
        {
            var sidePoint = (map.WorldToMap(GlobalPosition + new Vector2(i * (32 * direction), 0)) / 4);
            var spId = map.GetCellv(sidePoint);
            if (!_CheckOffroadTile(spId))
            {
                sidePoints++;
            }
        }
        return sidePoints;
    }

    private void SetTurnDirection()
    {
        // Check left
        var leftPoints = ScanSide(-1);
        var rightPoints = ScanSide(1);

        //GD.Print("leftpoints: " + leftPoints);
        //GD.Print("rightpoints: " + rightPoints);

        if (leftPoints > rightPoints)
        {
            AiDirection = Direction.LEFT;
            GD.Print("Go Left");
        }
        else if (rightPoints > leftPoints)
        {
            AiDirection = Direction.RIGHT;
            GD.Print("GO Right");
        }
        else
        {
            ScanForward();
        }
    }
}
