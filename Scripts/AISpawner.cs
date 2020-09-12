using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class AISpawner : Node2D
{
    [Export]
    public int AICarMaxAmount = 1;
    [Export]
    public float AIMinSpeed = -460;
    [Export]
    public float AIMaxSpeed = -460;
    [Export]
    public float SpawnTimeMin = 1.5f;
    [Export]
    public float SpawnTimeMax = 2.5f;
    PackedScene _aiCarScene;
    List<AICar> _aiCars;
    PlayerCar _playerCar { get; set; }
    TileMap _trackTileMap { get; set; }
    private bool _canSpawn;
    private Timer _spawnTimer;
    private int _screenW;
    private int _screenH;
    private Vector2 _previousSpawnPos;
    RandomNumberGenerator rng;
    private int[] _spawnTiles = new int[] { 0, 1, 2 };
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _spawnTimer = (Timer)GetNode("SpawnTimer");
        _aiCarScene = (PackedScene)ResourceLoader.Load("res://Scenes/AICar.tscn");
        _aiCars= new List<AICar>();
        rng = new RandomNumberGenerator();
        rng.Randomize();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        _CheckAICars();
        if (_trackTileMap != null && _canSpawn || _aiCars.Count < AICarMaxAmount / 2)
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
        aiCar.Set("Rng", rng);
        aiCar.Position = _GetSpawnLocation(_playerCar.Position, aiCar);

        if (aiCar.Position == Vector2.Zero)
        {
            aiCar.CallDeferred("QueueFree");
            return;
        }
        else 
        {
            _aiCars.Add(aiCar);
            aiCar.MaxSpeed = rng.RandfRange(AIMinSpeed, AIMaxSpeed);
            aiCar.Speed = rng.RandfRange(AIMinSpeed, aiCar.MaxSpeed);
            GetParent().AddChild(aiCar);
            _canSpawn = false;
            _spawnTimer.Start(rng.RandfRange(SpawnTimeMin, SpawnTimeMax));
            _previousSpawnPos = aiCar.Position;
        }
    }

    private void _CheckAICars()
    {
        foreach (AICar aiCar in _aiCars.ToList())
        {
            if (aiCar.Position.y - 1500 > _playerCar.Position.y || aiCar.Position.y < _playerCar.Position.y - 2500)
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

        if (calcCount < 128)
        {
            GD.Print("Created car at: " + spawnPos);
            return spawnPos;
        }

        return Vector2.Zero;
    }

    private bool _ValidateSpawnPosition(Vector2 spawnPos, AICar aiCar)
    {
        AICar.DiagonalData dd = aiCar.ScanDiagonals(spawnPos);
        int tileType = _GetTileType(spawnPos);

        return _spawnTiles.Contains(tileType) && dd.forwardPoints >= 20;
    }

    private Vector2 _CreateSpawnPosition(Vector2 playerPosition)
    {
        float factor = _screenW / 32;
        float screenTop = _playerCar.GetViewportTransform().AffineInverse().origin.y;

        float spawnPosY = rng.RandfRange(playerPosition.y + (_screenH * 2), screenTop - (_screenH * 2));
        while (spawnPosY >= screenTop - 250 && spawnPosY <= playerPosition.y + 250)
        {
            spawnPosY = rng.RandfRange(playerPosition.y - (_screenH), playerPosition.y - (_screenH * 2));
        }
        
        float spawnPosX = (rng.RandfRange(0, factor) * factor);

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
