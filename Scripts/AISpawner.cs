using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class AISpawner : Node2D
{
    [Export]
    public int AICarMaxAmount = 5;
    PackedScene _aiCarScene;
    List<AICar> _aiCars;
    PlayerCar playerCar { get; set; }
    public TileMap tm { get; set; }
    private bool _canSpawn;
    private Timer _spawnTimer;

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
        if (tm != null)
        {
            _SpawnAICars();
        }
        else
        {
            GD.Print("Map null in spawner");
        }
        if (playerCar == null)
        {
            GD.Print("player so null");
        }
        
    }
    public void SetMap(TileMap tilemap)
    {
        tm = tilemap;
    }

    private void _SpawnAICars()
    {
        if (_aiCars.Count < AICarMaxAmount && _canSpawn)
        {
            AICar aiCar = (AICar)_aiCarScene.Instance();
            aiCar.Set("Map", tm);
            //aiCar.SetMap(tm);
            aiCar.Position = new Vector2(playerCar.Position.x + 50, playerCar.Position.y - 200);
            _aiCars.Add(aiCar);
            GetParent().AddChild(aiCar);
            _canSpawn = false;
            _spawnTimer.Start();
        }
    }

    private void _CheckAICars()
    {
        foreach (AICar aiCar in _aiCars.ToList())
        {
            if (aiCar.Position.y - 500 > playerCar.Position.y)
            {
                _aiCars.Remove(aiCar);
                aiCar.QueueFree();
            }
        }
    }

    public void _onSpawnTimer_Timeout()
    {
        _canSpawn = true;
    }
}
