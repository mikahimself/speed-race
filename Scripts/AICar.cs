using Godot;
using System;

public class AICar : BaseCar
{
    public enum Direction {
        FORWARD,
        LEFT,
        RIGHT
    }

    public struct DiagonalData {
        /*Vector2 rightHit;
        Vector2 leftHit;
        int rightPoints;
        int leftPoints;*/
        public DiagonalData(Vector2 rh, Vector2 lh, int rp, int lp)
        {
            rightHit = rh;
            leftHit = lh;
            rightPoints = rp;
            leftPoints = lp;
        }
        public Vector2 rightHit { get; }
        public Vector2 leftHit { get; }
        public int rightPoints { get; }
        public int leftPoints { get; }

        public override string ToString() => $"RightHit: {rightHit.x}, {rightHit.y} ";
    }

    [Export]
    public Line2D line;
    public Direction AiDirection = Direction.FORWARD;
    public Line2D lineR;
    public Line2D lineL;

    public override void _Ready()
    {
        base._Ready();
        lineR = new Line2D();
        lineL = new Line2D();
        lineR.Points = new Vector2[] { GlobalPosition, GlobalPosition + new Vector2(160, -160) };
        lineL.Points = new Vector2[] { GlobalPosition, GlobalPosition + new Vector2(-160, -160) };
        AddChild(lineR);
        AddChild(lineL);

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
        DiagonalData dd = ScanDiagonals();
        lineR.ClearPoints();
        lineR.AddPoint(new Vector2(0, 0), 0);
        lineR.AddPoint(new Vector2(dd.rightHit.x, dd.rightHit.y), 1);
        lineR.DefaultColor = dd.rightPoints < 5 ? new Color(1, 0, 0, 1) : new Color(0, 0, 1, 1);
        
        lineL.ClearPoints();
        lineL.AddPoint(new Vector2(0, 0), 0);
        lineL.AddPoint(new Vector2(dd.leftHit.x, dd.leftHit.y), 1);
        lineL.DefaultColor = dd.leftPoints < 5 ? new Color(1, 0, 0, 1) : new Color(0, 0, 1, 1);
        
        if (_CheckOffroadTile(forwardPosId))
        {
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

    private DiagonalData ScanDiagonals()
    {
        var leftPoints = 0;
        var rightPoints = 0;
        bool lc = false;
        bool rc = false;
        var leftCutoff = new Vector2(-5 * 32, -5 * 32);
        var rightCutoff = new Vector2(5 * 32, -5 * 32);

        for (int i = -5; i <= 5; i++)
        {
            var sidePoint = (map.WorldToMap(GlobalPosition + new Vector2(i * 32, -(i * 32))) / 4);
            var spId = map.GetCellv(sidePoint);
            if (!_CheckOffroadTile(spId))
            {
                if (i < 0) {
                    leftPoints++;
                }
                else if (i > 0)
                {
                    rightPoints++;
                }
            }
            else
            {
                if (i < 0 && !lc) {
                    leftCutoff = new Vector2((i + 1) * 32, (i + 1) * 32);
                    lc = true;
                }
                else if (i > 0 && !rc)
                {
                    rightCutoff = new Vector2((i - 1) * 32, -((i - 1) * 32));
                    rc = true;
                }
            }
        }

        return new DiagonalData(rightCutoff, leftCutoff, rightPoints, leftPoints);
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
