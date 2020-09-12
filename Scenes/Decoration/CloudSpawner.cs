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

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _cloudScene = (PackedScene)ResourceLoader.Load("res://Scenes/Decoration/Cloud.tscn");
    }

    public void SetupSpawner(int screenWidth, int screenHeight)
    {
        _screenWidth = screenWidth;
        _screenHeight = screenHeight;
    }


    public void SpawnCloud()
    {
        if (_clouds.Count < MaxClouds)
        {
            Cloud cloud = (Cloud)_cloudScene.Instance();
            cloud.SetCloudPosition(0, _screenWidth, GetViewportTransform().AffineInverse().origin.y, rng);
            cloud.SetScaleAndSpeed(rng);
            GetParent().AddChild(cloud);
            _clouds.Add(cloud);
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
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
            if (cloud.Position.y > cloud.StartPosition.y + _screenHeight)
            {
                GD.Print("Removed cloud. Clouds on track: " + _clouds.Count);
                _clouds.Remove(cloud);
                cloud.CallDeferred("QueueFree");
            }
        }
    }
}
