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
        ScanForward();
    }

    private void SetTurnDirection()
    {
        // Check left
        var leftPoints = 0;
        var rightPoints = 0;
        for (int i = 1; i <= 5; i++)
        {
            var lp = (map.WorldToMap(GlobalPosition + new Vector2(i * -64, 0)) / 4);
            var rp = (map.WorldToMap(GlobalPosition + new Vector2(i *  64, 0)) / 4);
            var lpId = map.GetCellv(lp);
            var rpId = map.GetCellv(rp);
            if (_CheckOffroadTile(lpId))
            {
                leftPoints++;
            }
            if (_CheckOffroadTile(rpId))
            {
                rightPoints++;
            }
            if (leftPoints < rightPoints)
            {
                AiDirection = Direction.LEFT;
            }
            else
            {
                AiDirection = Direction.RIGHT;
            }
        }
    }
}
