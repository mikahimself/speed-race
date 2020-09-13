using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class CloudSpawner : Node2D
{
    [Export]
    public int MaxClouds = 8;
    private PackedScene _cloudScene;
    private Vector2 startPos;

    private int _screenWidth;
    private int _screenHeight;
    List<Cloud> _clouds = new List<Cloud>();
    RandomNumberGenerator rng = new RandomNumberGenerator();
    PlayerCar _playerCar;
    int xOffset = 200;
    int yDespawnOffset = 300;

    public override void _Ready()
    {
        _cloudScene = (PackedScene)ResourceLoader.Load("res://Scenes/Decoration/Cloud.tscn");
    }

    public void SetupSpawner(int screenWidth, int screenHeight, PlayerCar playerCar)
    {
        _screenWidth = screenWidth;
        _screenHeight = screenHeight;
        _playerCar = playerCar;
    }


    public void SpawnCloud()
    {
        if (_clouds.Count < MaxClouds)
        {
            Cloud cloud = (Cloud)_cloudScene.Instance();
            cloud.SetCloudPosition(-xOffset, _screenWidth + xOffset, GetViewportTransform().AffineInverse().origin.y, rng);
            cloud.SetScaleAndSpeed(rng);
            cloud.SetCloudFrame(rng);
            GetParent().AddChild(cloud);
            _clouds.Add(cloud);
        }
    }

    public override void _Process(float delta)
    {
        if (_clouds.Count < MaxClouds)
        {
            SpawnCloud();
        }
        _CheckClouds();
    }

    private void _CheckClouds()
    {
        foreach (Cloud cloud in _clouds.ToList())
        {
            if (cloud.Position.y > _playerCar.Position.y + yDespawnOffset)
            {
                _clouds.Remove(cloud);
                cloud.CallDeferred("QueueFree");
            }
        }
    }
}
