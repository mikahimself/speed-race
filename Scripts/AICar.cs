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
    public RandomNumberGenerator Rng { get; set; }
    
    private Direction _aiDirection = Direction.FORWARD;
    private Line2D _lineForward;
    private Line2D _lineRight;
    private Line2D _lineLeft;
    private Line2D[] _lines;
    private bool _randomTurn = false;
    private Color Green = new Color(0, 1, 0, 1);
    private Color Yellow = new Color(1, 1, 0, 1);
    private Color Red = new Color(1, 0, 0, 1);
    private Color Orange = new Color(1, 0.5f, 0, 1);
    private Timer _moveTimer;
    private Timer _turnTimer;
    private RayCast2D _rayLeft;
    private RayCast2D _rayRight;
    private RayCast2D _rayFront;
    private Sprite _carSprite;

    public override void _Ready()
    {
        base._Ready();
        onTrackTiles = new int[] {0, 1, 2, 16, 19, 35, 36, 37, 38, 40, 41, 42, 43, 45, 48, 49, 51, 52, 54, 55};
        _SetupTimers();
        _SetupRays();
        _SetupSprite();
    }

    private void _SetupTimers()
    {
        _moveTimer = (Timer)GetNode("MoveTimer");
        _turnTimer = (Timer)GetNode("TurnTimer");
        _moveTimer.Connect("timeout", this, "_onMoveTimerTimeout");
        _turnTimer.Connect("timeout", this, "_onTurnTimerTimeout");
    }

    private void _SetupRays()
    {
        _lineForward = (Line2D)GetNode("Line2Ds/LineForward");
        _lineRight = (Line2D)GetNode("Line2Ds/LineRight");
        _lineLeft = (Line2D)GetNode("Line2Ds/LineLeft");
        _lines = new Line2D[] { _lineRight, _lineLeft, _lineForward };
        
        _rayLeft = (RayCast2D)GetNode("RayCast2DLeft");
        _rayRight = (RayCast2D)GetNode("RayCast2DRight");
        _rayFront = (RayCast2D)GetNode("RayCast2DFront");
    }

    private void _SetupSprite()
    {
        Sprite car = (Sprite)GetNode("CarSprite");
        car.Frame = Rng.RandiRange(0, 1);
    }

    public void SpawnSetup()
    {
        
    }

    public override void GetControls(float delta)
    {
        DiagonalData dd = ScanDiagonals(GlobalPosition);
        
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
        //DrawDirections(dd);
    }

    private void SteerForward(DiagonalData dd)
    {
        SideSpeed = 0;

        if (_moveTimer.IsStopped())
        {
            _moveTimer.Start();
        }
       
        if (dd.forwardPoints <= 8 || _rayFront.IsColliding())
        {   
            SetTurnDirection(dd);
            return;
        }
        else if (dd.rightPoints <= 2 || dd.leftPoints <= 2)
        {
            SetTurnDirection(dd);
        }

        SetSpeed(dd);
    }

    private void SetSpeed(DiagonalData dd)
    {
        if ((dd.forwardPoints < 6 || _rayFront.IsColliding()) && Speed < MaxSpeed * 0.5f)
        {
            Speed += BrakeDeceleration;
        }
        else if (dd.forwardPoints <= 10 && Speed < MaxSpeed * 0.8f)
        {
            Speed += Deceleration;
        }
        else if (Speed > MaxSpeed) 
        {
            Speed -= Acceleration;
        }

        if (_aiDirection == Direction.LEFT)
        {
            SideSpeed = MaxSideSpeed * -1;
        }
        if (_aiDirection == Direction.RIGHT)
        {
            SideSpeed = MaxSideSpeed;
        }
    }

    private void SteerSideways(DiagonalData dd)
    {
        if (_aiDirection == Direction.LEFT)
        {
            if (dd.leftPoints < 4)
            {
                _aiDirection = Direction.FORWARD;
                //GD.Print("Switch from left to forward. Leftpoints: " + dd.leftPoints + " RP: " + dd.rightPoints);
            }
        }
        else if (_aiDirection == Direction.RIGHT)
        {
            if (dd.rightPoints < 4)
            {
                _aiDirection = Direction.FORWARD;
                //GD.Print("Switch from right to forward. Rightpoints: " + dd.rightPoints + " LP: " + dd.leftPoints);
            }
        }
        SetSpeed(dd);
    }

    public DiagonalData ScanDiagonals(Vector2 scanFromPosition)
    {
        var leftPoints = 0;
        var rightPoints = 0;
        var forwardPoints = 0;
        var leftCutoff = new Vector2(-5 * 32, -5 * 32);
        var rightCutoff = new Vector2(5 * 32, -5 * 32);
        var forwardCutoff = new Vector2(0, -5 * 32);
        bool foundOffRoad = false;

        if (Map != null)
        {
            while (!foundOffRoad && rightPoints < 15)
            {
                rightPoints++;
                // Subtract 32 from right side to account for tile origin pos.
                var sidePoint = (Map.WorldToMap(scanFromPosition + new Vector2(rightPoints * 32 - 32, -(rightPoints * 32))) / 4);
                var spId = Map.GetCellv(sidePoint);
                foundOffRoad = _CheckOffroadTile(spId);
            }
            rightCutoff = new Vector2((rightPoints - 1) * 32, -((rightPoints - 1) * 32));
            
            foundOffRoad = false;

            while (!foundOffRoad && leftPoints > -15)
            {
                leftPoints--;
                var sidePoint = (Map.WorldToMap(scanFromPosition + new Vector2(leftPoints * 32, leftPoints * 32)) / 4);
                var spId = Map.GetCellv(sidePoint);
                foundOffRoad = _CheckOffroadTile(spId);
            }
            leftCutoff = new Vector2((leftPoints + 1) * 32, (leftPoints + 1) * 32);

            foundOffRoad = false;

            while (!foundOffRoad && forwardPoints > -20)
            {
                forwardPoints--;
                var forwardPoint = (Map.WorldToMap(scanFromPosition + new Vector2(0, forwardPoints * 32)) / 4);
                var fpId = Map.GetCellv(forwardPoint);
                foundOffRoad = _CheckOffroadTile(fpId);
            }
            forwardCutoff = new Vector2(0, (forwardPoints + 1) * 32);
        }

        return new DiagonalData(rightCutoff, leftCutoff, forwardCutoff, rightPoints, Math.Abs(leftPoints), Math.Abs(forwardPoints));
    }

    public void DrawDirections(DiagonalData dd)
    {
        int[] limitPoints = new int[] { dd.rightPoints, dd.leftPoints, dd.forwardPoints };
        Vector2[] hitPoints = new Vector2[] { dd.rightHit, dd.leftHit, dd.forwardHit };

        for (int i = 0; i < _lines.Length; i++)
        {
            _lines[i].ClearPoints();
            _lines[i].AddPoint(Vector2.Zero, 0);
            _lines[i].AddPoint(hitPoints[i]);

            switch(limitPoints[i])
            {
                case 7: case 6: case 5:
                    _lines[i].DefaultColor = Yellow;
                    break;
                case 4: case 3:
                    _lines[i].DefaultColor = Orange;
                    break;
                case 2: case 1:
                    _lines[i].DefaultColor = Red;
                    break;
                default:
                    _lines[i].DefaultColor = Green;
                    break;        
            }
        }
    }

    private void SetTurnDirection(DiagonalData dd)
    {
        if (_randomTurn)
        {
            _aiDirection = _GetRandomTurnDirection(dd);
            return;
        }
        
        //GD.Print("LeftPoints: " + dd.leftPoints + " RightPoints: " + dd.rightPoints);

        if (dd.leftPoints > dd.rightPoints || dd.rightPoints <= 2)
        {
            _aiDirection = Direction.LEFT;
            /*if (dd.rightPoints + 5 < dd.leftPoints)
            {
                _aiDirection = Direction.LEFT;
                //_turnTimer.Start((float)GD.RandRange(1.0, 2.25));
                //GD.Print("Much more space on left " + dd);
            }
            else if (dd.rightPoints <= 2 && dd.leftPoints - dd.rightPoints > 1)
            {
                _aiDirection = Direction.LEFT;
                //_turnTimer.Start((float)GD.RandRange(1.0, 2.25));
                //GD.Print("Low Points - Go Left " + dd);
            }*/
            
        }
        
        else if (dd.rightPoints > dd.leftPoints || dd.leftPoints <= 2)
        {
            _aiDirection = Direction.RIGHT;
            /*if (dd.leftPoints + 5 < dd.rightPoints)
            {
                _aiDirection = Direction.RIGHT;
                //_turnTimer.Start((float)GD.RandRange(1.0, 2.25));
                //GD.Print("Much more space on right " + dd);
            }
            else if (dd.leftPoints <= 2 && dd.rightPoints - dd.leftPoints > 1)
            {
                _aiDirection = Direction.RIGHT;
                //_turnTimer.Start((float)GD.RandRange(1.0, 2.25));
                //GD.Print("GO Right: " + dd);
            }*/
            
        }
        //GD.Print("DIR NOW: " + _aiDirection);
    }

    private Direction _GetRandomTurnDirection(DiagonalData dd)
    {
        Direction randomDir = Direction.FORWARD;
        int randomizer = Rng.RandiRange(0, 12);

        // If tight on sides, continue forward.
        if (dd.rightPoints < 3 && dd.leftPoints < 3)
        {
            randomDir = Direction.FORWARD;
        }
        else if (dd.rightPoints <= 3 && dd.leftPoints > 3)
        {
            randomDir = Direction.LEFT;
        }
        else if (dd.leftPoints <= 3 && dd.rightPoints > 3)
        {
            randomDir = Direction.RIGHT;
        }
        // If enough space on both sides, get random direction (that may also be forward)
        else if (dd.rightPoints > 3 && dd.leftPoints > 3)
        {
            if (randomizer < 5)
            {
                randomDir = Direction.RIGHT;
            }
            else if (randomizer < 10)
            {
                randomDir = Direction.LEFT;
            }

            _randomTurn = false;
            _turnTimer.Start((float)GD.RandRange(1.5, 2.5));
        }

        return randomDir;
    }

    public void _onMoveTimerTimeout()
    {
        //GD.Print("Movetimer fired. Direction is " + _aiDirection);
        if (_aiDirection == Direction.FORWARD)
        {
            DiagonalData dd = ScanDiagonals(GlobalPosition);
            //GD.Print("Speed: " + _speed + " MaxSpeed: " + MaxSpeed + " FP: " + dd.forwardPoints + " RP: " + dd.rightPoints + " LP: " + dd.leftPoints + " DIR: " + _aiDirection);
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
    }
}
