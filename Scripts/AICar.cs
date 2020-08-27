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

        public override string ToString() => $"RightHit: {rightHit.x}, {rightHit.y} | LeftHit: {leftHit.x}, {leftHit.y} | RightPoints: {rightPoints} | LeftPoints: {leftPoints}";
    }

    [Export]
    public Line2D line;
    public Direction AiDirection = Direction.FORWARD;
    public Line2D lineR;
    public Line2D lineL;
    private bool _randomTurn = false;
    private Color green = new Color(0, 1, 0, 1);
    private Color yellow = new Color(1, 1, 0, 1);
    private Color red = new Color(1, 0, 0, 1);
    private Color orange = new Color(1, 0.5f, 0, 1);
    public Timer moveTimer;
    Timer turnTimer;

    public override void _Ready()
    {
        base._Ready();
        lineR = new Line2D();
        lineL = new Line2D();
        lineR.Points = new Vector2[] { GlobalPosition, GlobalPosition + new Vector2(160, -160) };
        lineL.Points = new Vector2[] { GlobalPosition, GlobalPosition + new Vector2(-160, -160) };
        AddChild(lineR);
        AddChild(lineL);
        moveTimer = (Timer)GetNode("MoveTimer");
        moveTimer.Connect("timeout", this, "_onMoveTimerTimeout");
        turnTimer = (Timer)GetNode("TurnTimer");
        turnTimer.Connect("timeout", this, "_onTurnTimerTimeout");

        line = (Line2D)GetNode("Line2D");
        line.Points = new Vector2[] { GlobalPosition, GlobalPosition + new Vector2(0, -320)};
    }

    public override void GetControls(float delta)
    {
        DiagonalData dd = ScanDiagonals();
        switch (AiDirection)
        {
            case Direction.FORWARD:
                SteerForward(dd);
                break;
            case Direction.LEFT:
                SteerSideways(dd);
                break;
            case Direction.RIGHT:
                SteerSideways(dd);
                break;
            default:
                SteerForward(dd);
                break;
        }
    }

    private void SteerForward(DiagonalData dd)
    {
        if (moveTimer.IsStopped())
        {
            moveTimer.Start();
        }
        SideSpeed = 0;
        var forwardPos = (map.WorldToMap(GlobalPosition + new Vector2(0, -320)) / 4);
        var forwardPosId = map.GetCellv(forwardPos);
       
        if (_CheckOffroadTile(forwardPosId))
        {
            SetTurnDirection(dd);
            return;
        }
        else if (dd.rightPoints <= 2 || dd.leftPoints <= 2)
        {
            SetTurnDirection(dd);
        }
        else
        {
            line.DefaultColor = new Color(0, 0, 1, 1);
        }
        _speed = MaxSpeed + 0;
    }

    private void ScanForward()
    {
        var forwardPos = (map.WorldToMap(GlobalPosition + new Vector2(0, -320)) / 4);
        var forwardPosId = map.GetCellv(forwardPos);
        if (!_CheckOffroadTile(forwardPosId))
        {
            //GD.Print("Go forward");
            AiDirection = Direction.FORWARD;
        }
        
    }

    private void SteerSideways(DiagonalData dd)
    {
        //int direction = dd.rightPoints > dd.leftPoints ? 1 : -1;
        
        _speed = MaxSpeed + 0;

        if (AiDirection == Direction.LEFT )
        {
            SideSpeed = MaxSideSpeed * -1;
            if (dd.leftPoints <= 3) {
              //  GD.Print("Left points running low.");
                ScanForward();
            }
        }
        if (AiDirection == Direction.RIGHT)
        {
            SideSpeed = MaxSideSpeed;
            if (dd.rightPoints <= 3) {
                //GD.Print("Right points running low.");
                ScanForward();
            }
        }
    }

    private DiagonalData ScanDiagonals()
    {
        var leftPoints = 0;
        var rightPoints = 0;
        var leftCutoff = new Vector2(-5 * 32, -5 * 32);
        var rightCutoff = new Vector2(5 * 32, -5 * 32);
        bool foundOffRoad = false;

        while (!foundOffRoad && rightPoints < 15)
        {
            rightPoints++;
            var sidePoint = (map.WorldToMap(GlobalPosition + new Vector2(rightPoints * 32, -(rightPoints * 32))) / 4);
            var spId = map.GetCellv(sidePoint);
            foundOffRoad = _CheckOffroadTile(spId);
        }
        rightCutoff = new Vector2((rightPoints - 1) * 32, -((rightPoints - 1) * 32));
        
        foundOffRoad = false;

        while (!foundOffRoad && leftPoints > -15)
        {
            leftPoints--;
            var sidePoint = (map.WorldToMap(GlobalPosition + new Vector2(leftPoints * 32, leftPoints * 32)) / 4);
            var spId = map.GetCellv(sidePoint);
            foundOffRoad = _CheckOffroadTile(spId);
        }
        leftCutoff = new Vector2((leftPoints + 1) * 32, (leftPoints + 1) * 32);
        
        return new DiagonalData(rightCutoff, leftCutoff, rightPoints, Math.Abs(leftPoints));
    }

    public void DrawDiagonals(DiagonalData dd)
    {
        lineR.ClearPoints();
        lineR.AddPoint(new Vector2(0, 0), 0);
        lineR.AddPoint(new Vector2(dd.rightHit.x, dd.rightHit.y), 1);

        lineL.ClearPoints();
        lineL.AddPoint(new Vector2(0, 0), 0);
        lineL.AddPoint(new Vector2(dd.leftHit.x, dd.leftHit.y), 1);
        
        switch (dd.rightPoints)
        {
            case 3:
                lineR.DefaultColor = yellow;
                break;
            case 2:
                lineR.DefaultColor = orange;
                break;
            case 1:
                lineR.DefaultColor = red;
                break;
            default:
                lineR.DefaultColor = green;
                break;
        }

        switch (dd.leftPoints)
        {
            case 3:
                lineL.DefaultColor = yellow;
                break;
            case 2:
                lineL.DefaultColor = orange;
                break;
            case 1:
                lineL.DefaultColor = red;
                break;
            default:
                lineL.DefaultColor = green;
                break;
        }
    }

    private void SetTurnDirection(DiagonalData dd)
    {
        if (dd.rightPoints > 3 && dd.leftPoints > 3 && _randomTurn)
        {
            if (GD.RandRange(0, 10) < 5)
            {
                AiDirection = Direction.RIGHT;
                _randomTurn = false;
                turnTimer.Start((float)GD.RandRange(0.5, 2.25));
                //GD.Print("felt like turning right");
            }
            else
            {
                AiDirection = Direction.LEFT;
                _randomTurn = false;
                turnTimer.Start((float)GD.RandRange(0.5, 2.25));
                //GD.Print("felt like turning left");
            }
                
        }
        else if (dd.leftPoints > dd.rightPoints)
        {
            if (dd.rightPoints <= 2 && dd.leftPoints - dd.rightPoints > 1)
            {
                AiDirection = Direction.LEFT;
                turnTimer.Start((float)GD.RandRange(0.5, 2.25));
                //GD.Print("Low Points - Go Left " + dd);
            } 
            else if (dd.rightPoints + 7 < dd.leftPoints)
            {
                AiDirection = Direction.LEFT;
                turnTimer.Start((float)GD.RandRange(0.5, 2.25));
                //GD.Print("Much more space on left " + dd);
            }
        }
        
        else if (dd.rightPoints > dd.leftPoints)
        {
            if (dd.leftPoints <= 2 && dd.rightPoints - dd.leftPoints > 1)
            {
                AiDirection = Direction.RIGHT;
                turnTimer.Start((float)GD.RandRange(0.5, 2.25));
                //GD.Print("GO Right: " + dd);
            }
            else if (dd.leftPoints + 7 < dd.rightPoints)
            {
                AiDirection = Direction.RIGHT;
                turnTimer.Start((float)GD.RandRange(0.5, 2.25));
                //GD.Print("Much more space on right " + dd);
            }
            
        }
        else
        {
            ScanForward();
        }
    }

    public void _onMoveTimerTimeout()
    {
        //GD.Print("Timer Timeout, direction: " + AiDirection);
        if (AiDirection == Direction.FORWARD)
        {
            DiagonalData dd = ScanDiagonals();
            _randomTurn = true;
            SetTurnDirection(dd);
        }
        
    }

    public void _onTurnTimerTimeout()
    {
        AiDirection = Direction.FORWARD;
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        DiagonalData dd = ScanDiagonals();
        DrawDiagonals(dd);
    }
}
