using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class AISpawner : Node2D
{
    [Export]
    public int AICarMaxAmount = 10;
    PackedScene _aiCarScene;
    List<AICar> _aiCars;
    PlayerCar _playerCar { get; set; }
    TileMap _trackTileMap { get; set; }
    private bool _canSpawn;
    private Timer _spawnTimer;
    private int _screenW;
    private int _screenH;
    private int[] _spawnTiles = new int[] { 0, 1, 2, 16, 19 };
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _spawnTimer = (Timer)GetNode("SpawnTimer");
        _aiCarScene = (PackedScene)ResourceLoader.Load("res://Scenes/AICar.tscn");
        _aiCars= new List<AICar>();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        _CheckAICars();
        if (_trackTileMap != null && _canSpawn || _aiCars.Count < 5)
        {
            _SpawnAICars();
        }
    }
    public void SetValues(TileMap tilemap, PlayerCar playerCar, int screenW, int screenH)
    {
        _trackTileMap = tilemap;
        _playerCar = playerCar;
        _screenW = screenW;
        _screenH = screenH;
    }

    private void _SpawnAICars()
    {
        AICar aiCar = (AICar)_aiCarScene.Instance();
        aiCar.Set("Map", _trackTileMap);
        aiCar.Position = _GetSpawnLocation(_playerCar.Position, aiCar);

        if (aiCar.Position == Vector2.Zero)
        {
            return;
        }
        else 
        {
            GD.Print("Add car at " + aiCar.Position);
            _aiCars.Add(aiCar);
            aiCar.MaxSpeed = (float)GD.RandRange(-375, -649);
            GetParent().AddChild(aiCar);
            _canSpawn = false;
            _spawnTimer.Start((float)GD.RandRange(1.0, 2.0));
        }
    }

    private void _CheckAICars()
    {
        foreach (AICar aiCar in _aiCars.ToList())
        {
            if (aiCar.Position.y - 500 > _playerCar.Position.y)
            {
                GD.Print("Removed car. Cars on track: " + _aiCars.Count);
                _aiCars.Remove(aiCar);
                aiCar.CallDeferred("QueueFree");
            }
        }
    }

    private Vector2 _GetSpawnLocation(Vector2 playerPosition, AICar aICar)
    {
        float spawnPosY = (float)GD.RandRange(playerPosition.y -600, playerPosition.y -7500);
        float spawnPosX = (float)GD.RandRange(0, _screenW);
        int calcCount = 0;
        AICar.DiagonalData dd = aICar.ScanDiagonals(new Vector2(spawnPosX, spawnPosY));

        while (!Array.Exists(_spawnTiles, element => element == _GetTileType(new Vector2(spawnPosX, spawnPosY))) && 
                calcCount < 50 && 
                dd.forwardPoints < 5
                )
        {
            spawnPosX = (float)GD.RandRange(0, _screenW);
            dd = aICar.ScanDiagonals(new Vector2(spawnPosX, spawnPosY));
            calcCount++;
        }

        aICar.Position = new Vector2(spawnPosX, spawnPosY);
       

        if (calcCount < 50)
        {
            GD.Print("Found position: " + new Vector2(spawnPosX, spawnPosY) + " Forward points: " + dd.forwardPoints + " calc: " + calcCount);
            return new Vector2(spawnPosX, spawnPosY);
        }
        else
        {
            return Vector2.Zero;
        }
    }

    private int _GetTileType(Vector2 position)
    {
        var posOnMap = (_trackTileMap.WorldToMap(position) / 4);
        return _trackTileMap.GetCellv(posOnMap);
    }

    public void _onSpawnTimer_Timeout()
    {
        if (_aiCars.Count < AICarMaxAmount)
        {
            _canSpawn = true;
        }
        GD.Print("Cars on track: " + _aiCars.Count);
    }
}
