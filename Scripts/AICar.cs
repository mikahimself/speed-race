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
        public DiagonalData(Vector2 rh, Vector2 lh, Vector2 fh, int rp, int lp, int fp)
        {
            rightHit = rh;
            leftHit = lh;
            rightPoints = rp;
            leftPoints = lp;
            forwardHit = fh;
            forwardPoints = fp;
        }
        public Vector2 rightHit { get; }
        public Vector2 leftHit { get; }
        public Vector2 forwardHit { get; }
        public int rightPoints { get; }
        public int leftPoints { get; }
        public int forwardPoints { get; }

        public override string ToString() => $"RightHit: {rightHit.x}, {rightHit.y} | LeftHit: {leftHit.x}, {leftHit.y} | RightPoints: {rightPoints} | LeftPoints: {leftPoints}";
    }

    [Export]
    public Texture CpuTexture;
    
    private Direction _aiDirection = Direction.FORWARD;
    private Line2D _lineForward;
    private Line2D _lineRight;
    private Line2D _lineLeft;
    private bool _randomTurn = false;
    private Color Green = new Color(0, 1, 0, 1);
    private Color Yellow = new Color(1, 1, 0, 1);
    private Color Red = new Color(1, 0, 0, 1);
    private Color Orange = new Color(1, 0.5f, 0, 1);
    private Timer _moveTimer;
    private Timer _turnTimer;
    private RayCast2D _rayLeft;
    private RayCast2D _rayRight;

    public override void _Ready()
    {
        base._Ready();
        GD.Randomize();

        // Setup timers
        _moveTimer = (Timer)GetNode("MoveTimer");
        _turnTimer = (Timer)GetNode("TurnTimer");
        _moveTimer.Connect("timeout", this, "_onMoveTimerTimeout");
        _turnTimer.Connect("timeout", this, "_onTurnTimerTimeout");

        _lineForward = (Line2D)GetNode("Line2Ds/LineForward");
        _lineRight = (Line2D)GetNode("Line2Ds/LineRight");
        _lineLeft = (Line2D)GetNode("Line2Ds/LineLeft");
        
        _rayLeft = (RayCast2D)GetNode("RayCast2DLeft");
        _rayRight = (RayCast2D)GetNode("RayCast2DRight");
        if (CpuTexture != null)
        {
            Sprite car = (Sprite)GetNode("CarSprite");
            car.Texture = CpuTexture;
        }
    }

    public override void GetControls(float delta)
    {
        DiagonalData dd = ScanDiagonals();
        switch (_aiDirection)
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
        SideSpeed = 0;
        _speed = MaxSpeed;

        if (_moveTimer.IsStopped())
        {
            _moveTimer.Start();
        }
        
        Vector2 forwardPos = (Map.WorldToMap(GlobalPosition + new Vector2(0, -320)) / 4);
        int forwardPosId = Map.GetCellv(forwardPos);
       
        if (dd.forwardPoints <= 12)
        {
            _speed = MaxSpeed - 10f;
            SetTurnDirection(dd);
            return;
        }
        else if (dd.rightPoints <= 2 || dd.leftPoints <= 2)
        {
            SetTurnDirection(dd);
        }
    }

    private void ScanForward(DiagonalData dd)
    {
        if (dd.forwardPoints > 5)
        {
            _aiDirection = Direction.FORWARD;
        }
    }

    private void SteerSideways(DiagonalData dd)
    {
        _speed = MaxSpeed;

        if (_aiDirection == Direction.LEFT )
        {
            SideSpeed = MaxSideSpeed * -1;
            if (dd.leftPoints <= 3 || _rayLeft.IsColliding()) {
                _aiDirection = Direction.FORWARD;
            }
        }
        if (_aiDirection == Direction.RIGHT)
        {
            SideSpeed = MaxSideSpeed;
            if (dd.rightPoints <= 3 || _rayRight.IsColliding()) {
                _aiDirection = Direction.FORWARD;
            }
        }
    }

    private DiagonalData ScanDiagonals()
    {
        var leftPoints = 0;
        var rightPoints = 0;
        var forwardPoints = 0;
        var leftCutoff = new Vector2(-5 * 32, -5 * 32);
        var rightCutoff = new Vector2(5 * 32, -5 * 32);
        var forwardCutoff = new Vector2(0, -5 * 32);
        bool foundOffRoad = false;

        while (!foundOffRoad && rightPoints < 15)
        {
            rightPoints++;
            var sidePoint = (Map.WorldToMap(GlobalPosition + new Vector2(rightPoints * 32, -(rightPoints * 32))) / 4);
            var spId = Map.GetCellv(sidePoint);
            foundOffRoad = _CheckOffroadTile(spId);
        }
        rightCutoff = new Vector2((rightPoints - 1) * 32, -((rightPoints - 1) * 32));
        
        foundOffRoad = false;

        while (!foundOffRoad && leftPoints > -15)
        {
            leftPoints--;
            var sidePoint = (Map.WorldToMap(GlobalPosition + new Vector2(leftPoints * 32, leftPoints * 32)) / 4);
            var spId = Map.GetCellv(sidePoint);
            foundOffRoad = _CheckOffroadTile(spId);
        }
        leftCutoff = new Vector2((leftPoints + 1) * 32, (leftPoints + 1) * 32);

        foundOffRoad = false;

        while (!foundOffRoad && forwardPoints > -15)
        {
            forwardPoints--;
            var forwardPoint = (Map.WorldToMap(GlobalPosition + new Vector2(0, forwardPoints * 32)) / 4);
            var fpId = Map.GetCellv(forwardPoint);
            foundOffRoad = _CheckOffroadTile(fpId);
        }
        forwardCutoff = new Vector2(0, (forwardPoints + 1) * 32);
        
        return new DiagonalData(rightCutoff, leftCutoff, forwardCutoff, rightPoints, Math.Abs(leftPoints), Math.Abs(forwardPoints));
    }

    public void DrawDirections(DiagonalData dd)
    {
        _lineRight.ClearPoints();
        _lineRight.AddPoint(new Vector2(0, 0), 0);
        _lineRight.AddPoint(new Vector2(dd.rightHit.x, dd.rightHit.y), 1);

        _lineLeft.ClearPoints();
        _lineLeft.AddPoint(new Vector2(0, 0), 0);
        _lineLeft.AddPoint(new Vector2(dd.leftHit.x, dd.leftHit.y), 1);

        _lineForward.ClearPoints();
        _lineForward.AddPoint(new Vector2(0, 0), 0);
        _lineForward.AddPoint(new Vector2(dd.forwardHit.x, dd.forwardHit.y), 1);
        
        switch (dd.rightPoints)
        {
            case 3:
                _lineRight.DefaultColor = Yellow;
                break;
            case 2:
                _lineRight.DefaultColor = Orange;
                break;
            case 1:
                _lineRight.DefaultColor = Red;
                break;
            default:
                _lineRight.DefaultColor = Green;
                break;
        }

        switch (dd.leftPoints)
        {
            case 3:
                _lineLeft.DefaultColor = Yellow;
                break;
            case 2:
                _lineLeft.DefaultColor = Orange;
                break;
            case 1:
                _lineLeft.DefaultColor = Red;
                break;
            default:
                _lineLeft.DefaultColor = Green;
                break;
        }
        
        switch (dd.forwardPoints)
        {
            case 3:
                _lineForward.DefaultColor = Yellow;
                break;
            case 2:
                _lineForward.DefaultColor = Orange;
                break;
            case 1:
                _lineForward.DefaultColor = Red;
                break;
            default:
                _lineForward.DefaultColor = Green;
                break;
        }
    }

    private void SetTurnDirection(DiagonalData dd)
    {
        if (dd.rightPoints > 3 && dd.leftPoints > 3 && _randomTurn)
        {
            if (dd.leftPoints > dd.rightPoints + 5)
            {
                _aiDirection = Direction.LEFT;
            }
            else if (dd.rightPoints > dd.leftPoints + 5)
            {
                _aiDirection = Direction.RIGHT;
            }

            int turnDirRandomizer = (int)GD.RandRange(0, 11);
            if (turnDirRandomizer < 5)
            {
                _aiDirection = Direction.RIGHT;
                //GD.Print("felt like turning right");
            }
            else if (turnDirRandomizer < 10)
            {
                _aiDirection = Direction.LEFT;
                //GD.Print("felt like turning left");
            }
            else
            {
                _aiDirection = Direction.FORWARD;
            }

            _randomTurn = false;
            _turnTimer.Start((float)GD.RandRange(0.5, 2.25));
                
        }
        else if (dd.leftPoints > dd.rightPoints)
        {
            
            if (dd.rightPoints + 5 < dd.leftPoints)
            {
                _aiDirection = Direction.LEFT;
                _turnTimer.Start((float)GD.RandRange(0.5, 2.25));
                //GD.Print("Much more space on left " + dd);
            }
            else if (dd.rightPoints <= 2 && dd.leftPoints - dd.rightPoints > 1)
            {
                _aiDirection = Direction.LEFT;
                _turnTimer.Start((float)GD.RandRange(0.5, 2.25));
                //GD.Print("Low Points - Go Left " + dd);
            } 
            
        }
        
        else if (dd.rightPoints > dd.leftPoints)
        {
            if (dd.leftPoints + 5 < dd.rightPoints)
            {
                _aiDirection = Direction.RIGHT;
                _turnTimer.Start((float)GD.RandRange(0.5, 2.25));
                //GD.Print("Much more space on right " + dd);
            }
            else if (dd.leftPoints <= 2 && dd.rightPoints - dd.leftPoints > 1)
            {
                _aiDirection = Direction.RIGHT;
                _turnTimer.Start((float)GD.RandRange(0.5, 2.25));
                //GD.Print("GO Right: " + dd);
            }
            
        }
    }

    public void _onMoveTimerTimeout()
    {
        if (_aiDirection == Direction.FORWARD)
        {
            DiagonalData dd = ScanDiagonals();
            _randomTurn = true;
            SetTurnDirection(dd);
        }
    }

    public void _onTurnTimerTimeout()
    {
        _aiDirection = Direction.FORWARD;
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        DiagonalData dd = ScanDiagonals();
        //DrawDirections(dd);
    }
}
