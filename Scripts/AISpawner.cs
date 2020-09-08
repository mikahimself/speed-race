using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class AISpawner : Node2D
{
    [Export]
    public int AICarMaxAmount = 10;
    [Export]
    public double AIMinSpeed = -450;
    [Export]
    public double AIMaxSpeed = -640;
    [Export]
    public double SpawnTimeMin = 1.5;
    [Export]
    public double SpawnTimeMax = 2.5;
    PackedScene _aiCarScene;
    List<AICar> _aiCars;
    PlayerCar _playerCar { get; set; }
    TileMap _trackTileMap { get; set; }
    private bool _canSpawn;
    private Timer _spawnTimer;
    private int _screenW;
    private int _screenH;
    private Vector2 _previousSpawnPos;
    private int[] _spawnTiles = new int[] { 0, 1, 2 };
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
            aiCar.MaxSpeed = (float)GD.RandRange(AIMinSpeed, AIMaxSpeed);
            aiCar.onTrackTiles = new int[] {0, 1, 2, 16, 19};
            GetParent().AddChild(aiCar);
            _canSpawn = false;
            _spawnTimer.Start((float)GD.RandRange(SpawnTimeMin, SpawnTimeMax));
            _previousSpawnPos = aiCar.Position;
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

    private Vector2 _GetSpawnLocation(Vector2 playerPosition, AICar aiCar)
    {
        int calcCount = 0;
        Vector2 spawnPos = _CreateSpawnPosition(playerPosition);
        
        while (!_ValidateSpawnPosition(spawnPos, aiCar) && calcCount < 128)
        {
            spawnPos = _CreateSpawnPosition(playerPosition);
            calcCount++;
        }

        AICar.DiagonalData dd = aiCar.ScanDiagonals(spawnPos);
        if (calcCount < 128)
        {
            GD.Print("Found position: " + spawnPos + " Forward points: " + dd.forwardPoints + " calc: " + calcCount);
            return spawnPos;
        }
        else
        {
            return Vector2.Zero;
        }
    }

    private bool _ValidateSpawnPosition(Vector2 spawnPos, AICar aiCar)
    {
        AICar.DiagonalData dd = aiCar.ScanDiagonals(spawnPos);
        int tileType = _GetTileType(spawnPos);

        return _spawnTiles.Contains(tileType) && dd.forwardPoints > 10;
    }

    private Vector2 _CreateSpawnPosition(Vector2 playerPosition)
    {
        float factor = _screenW / 32;
        int spawnPosY = (int)GD.RandRange(playerPosition.y - (_screenH * 3), playerPosition.y - (_screenH * 7));
        int spawnPosX = (int)(GD.RandRange(0, factor) * factor);

        return new Vector2(spawnPosX, spawnPosY);
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
