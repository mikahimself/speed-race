using Godot;
using System;

public class AICar : BaseCar
{
    public enum Direction {
        FORWARD,
        LEFT,
        RIGHT
    }

    public Direction AiDirection = Direction.FORWARD;
        
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
        var forwardPos = (map.WorldToMap(GlobalPosition + new Vector2(0, -256)) / 4);
        var forwardPosId = map.GetCellv(forwardPos);
        if (_CheckOffroadTile(forwardPosId))
        {
            SetTurnDirection();
            return;
        }
        _speed = MaxSpeed + 400;
    }

    private void ScanForward()
    {
        var forwardPos = (map.WorldToMap(GlobalPosition + new Vector2(0, -512)) / 4);
        var forwardPosId = map.GetCellv(forwardPos);
        if (!_CheckOffroadTile(forwardPosId))
        {
            AiDirection = Direction.FORWARD;
        }
    }

    private void SteerSideways(int direction)
    {
        SideSpeed = MaxSideSpeed * direction;
        _speed = MaxSpeed + 400;
        SetTurnDirection();
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

        GD.Print("leftpoints: " + leftPoints);
        GD.Print("rightpoints: " + rightPoints);

        if (leftPoints > rightPoints)
        {
            AiDirection = Direction.LEFT;
        }
        else if (rightPoints > leftPoints)
        {
            AiDirection = Direction.RIGHT;
        }
        else
        {
            ScanForward();
        }
    }
}
