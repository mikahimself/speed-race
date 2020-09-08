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

    public override void _Ready()
    {
        base._Ready();
        GD.Randomize();
        onTrackTiles = new int[] {0, 1, 2, };

        // Setup timers
        _moveTimer = (Timer)GetNode("MoveTimer");
        _turnTimer = (Timer)GetNode("TurnTimer");
        _moveTimer.Connect("timeout", this, "_onMoveTimerTimeout");
        _turnTimer.Connect("timeout", this, "_onTurnTimerTimeout");

        _lineForward = (Line2D)GetNode("Line2Ds/LineForward");
        _lineRight = (Line2D)GetNode("Line2Ds/LineRight");
        _lineLeft = (Line2D)GetNode("Line2Ds/LineLeft");
        _lines = new Line2D[] { _lineRight, _lineLeft, _lineForward };
        
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
    }

    private void SteerForward(DiagonalData dd)
    {
        SideSpeed = 0;

        if (_moveTimer.IsStopped())
        {
            _moveTimer.Start();
        }
       
        if (dd.forwardPoints <= 8 && dd.rightPoints > 2 && dd.leftPoints > 2)
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

    private void ScanForward(DiagonalData dd)
    {
        if (dd.forwardPoints > 5)
        {
            _aiDirection = Direction.FORWARD;
        }
    }

    private void SetSpeed(DiagonalData dd)
    {
        if (dd.forwardPoints < 6 && _speed < -400)
        {
            _speed += BrakeDeceleration;
        }
        else if (dd.forwardPoints <= 10 && _speed < -475)
        {
            _speed += Deceleration;
        }
        else if (_speed > MaxSpeed) 
        {
            _speed -= Acceleration;
        }

        if (_speed > -200) {
            _speed = -200;
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
        if (_aiDirection == Direction.LEFT )
        {
            if (dd.leftPoints <= 3 || (_rayLeft.IsColliding() && dd.forwardPoints > 8)) {
                _aiDirection = Direction.FORWARD;
            }
        }
        else if (_aiDirection == Direction.RIGHT)
        {
            if (dd.rightPoints <= 3 || (_rayRight.IsColliding() && dd.forwardPoints > 8)) {
                _aiDirection = Direction.FORWARD;
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
                var sidePoint = (Map.WorldToMap(scanFromPosition + new Vector2(rightPoints * 32, -(rightPoints * 32))) / 4);
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

            while (!foundOffRoad && forwardPoints > -15)
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
                case 3:
                    _lines[i].DefaultColor = Yellow;
                    break;
                case 2:
                    _lines[i].DefaultColor = Orange;
                    break;
                case 1:
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
        else if (dd.leftPoints >= dd.rightPoints)
        {
            
            if (dd.rightPoints + 5 < dd.leftPoints)
            {
                _aiDirection = Direction.LEFT;
                _turnTimer.Start((float)GD.RandRange(1.0, 2.25));
                //GD.Print("Much more space on left " + dd);
            }
            else if (dd.rightPoints <= 2 && dd.leftPoints - dd.rightPoints > 1)
            {
                _aiDirection = Direction.LEFT;
                _turnTimer.Start((float)GD.RandRange(1.0, 2.25));
                //GD.Print("Low Points - Go Left " + dd);
            }
            
        }
        
        else if (dd.rightPoints >= dd.leftPoints)
        {
            if (dd.leftPoints + 5 < dd.rightPoints)
            {
                _aiDirection = Direction.RIGHT;
                _turnTimer.Start((float)GD.RandRange(1.0, 2.25));
                //GD.Print("Much more space on right " + dd);
            }
            else if (dd.leftPoints <= 2 && dd.rightPoints - dd.leftPoints > 1)
            {
                _aiDirection = Direction.RIGHT;
                _turnTimer.Start((float)GD.RandRange(1.0, 2.25));
                //GD.Print("GO Right: " + dd);
            }
            
        }
    }

    public void _onMoveTimerTimeout()
    {
        if (_aiDirection == Direction.FORWARD)
        {
            DiagonalData dd = ScanDiagonals(GlobalPosition);
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
        DiagonalData dd = ScanDiagonals(GlobalPosition);
        //DrawDirections(dd);
        if (_speed > -100)
        {
            GD.Print("**************");
            GD.Print("Speed: " + _speed + " FP: " + dd.forwardPoints + " RP: " + dd.rightPoints + " LP: " + dd.leftPoints + " DIR: " + _aiDirection);
            GD.Print("POS: " + GlobalPosition);
        }
    }
}
