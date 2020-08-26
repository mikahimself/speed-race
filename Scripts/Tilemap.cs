using Godot;
using System;

public class Tilemap : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export]
    public int scrollSpeed = 4;
    private int _screenH;
    private int _screenW;
    private int _tileSize = 64;
    TileMap tm;
    Camera2D cam;
    KinematicBody2D car;
    KinematicBody2D aicar;
    
    int currentHeight = 0;

    uint mapstartTime;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _screenW = (int)GetViewport().Size.x;
        _screenH = (int)GetViewport().Size.y;
        tm = (TileMap)GetNode("TileMap");
        var timestart = OS.GetTicksMsec();
        SetTiles();
        var timenow = OS.GetTicksMsec();
        var elapsed = timenow - timestart;
        cam = (Camera2D)GetNode("PlayerCar/Camera2D");
        cam.EditorDrawDragMargin = true;
        car = (KinematicBody2D)GetNode("PlayerCar");
        aicar = (KinematicBody2D)GetNode("AICar");
        car.Position = new Vector2(_screenW / 2, _screenH / 2 - 400);
        aicar.Position = new Vector2(_screenW / 2 - 200, _screenH / 2 - 400);

        GD.Print("elapsed: " + elapsed);
        mapstartTime = OS.GetTicksMsec();
    }

    public void SetTiles()
    {
        for (int i = 0; i < TileMapParts.track.Count; i++)
        {
            SetTileParts(TileMapParts.track[i], i);
        }
    }

    public void AddTiles()
    {
        TileMapParts.track.Add((int)GD.RandRange(0, 4));
        SetTileParts(TileMapParts.track[TileMapParts.track.Count -1], 1);
        if (TileMapParts.track.Count > 5) 
        {
            TileMapParts.track.RemoveAt(0);
        }
    }

    async public void SetTileParts(int part, int height)
    {
        currentHeight -= TileMapParts.mapParts[part].Length;
        for (int i = 0; i < TileMapParts.mapParts[part].Length; i++)
        {
            for (int j = 0; j < TileMapParts.mapParts[part][i].Length; j++)
            {
                tm.SetCell(j, currentHeight + i, TileMapParts.mapParts[part][i][j]);
//                await ToSignal(GetTree().CreateTimer(0.01f), "timeout");
            }
        }
    }

    public override void _Process(float delta)
    {
        if (car.Position.y - _screenH < currentHeight * _tileSize)
        {
            AddTiles();
        }
    }
}
