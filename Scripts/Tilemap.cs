using Godot;
using System;

public class Tilemap : Node2D
{
    private int _screenH;
    private int _screenW;
    private int _tileSize = 64;
    TileMap TileMapTrack;
    TileMap TileMapDecor;
    Camera2D cam;
    KinematicBody2D PlayerCar;
    KinematicBody2D AICar;
    KinematicBody2D AICar2;
    KinematicBody2D AICar3;
    
    public int CurrentMapHeight = 0;

    private uint _mapstartTime;

    private int[] _decorationTiles = new int[] {
        0, 1, 2, 3, -1, -1, -1, -1
    }; 
    Timer pt;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _screenW = (int)GetViewport().Size.x;
        _screenH = (int)GetViewport().Size.y;
        TileMapTrack = (TileMap)GetNode("TileMapTrack");
        TileMapDecor = (TileMap)GetNode("TileMapDecoration");
        pt = (Timer)GetNode("PlayTimer");
        pt.Connect("timeout", this, "_OnPlayTimerTimeout");
        SetTiles();
        cam = (Camera2D)GetNode("PlayerCar/Camera2D");
        //cam.EditorDrawDragMargin = true;
        PlayerCar = (KinematicBody2D)GetNode("PlayerCar");
        PlayerCar.Set("Map", TileMapTrack);
        AICar = (KinematicBody2D)GetNode("AICar1");
        AICar.Set("Map", TileMapTrack);
        AICar2 = (KinematicBody2D)GetNode("AICar2");
        AICar2.Set("Map", TileMapTrack);
        AICar3 = (KinematicBody2D)GetNode("AICar3");
        AICar3.Set("Map", TileMapTrack);
        PlayerCar.Position = new Vector2(_screenW / 2, _screenH / 2 - 400);
        AICar.Position = new Vector2(_screenW / 2 - 200, _screenH / 2 - 400);
        AICar2.Position = new Vector2(_screenW / 2 + 200, _screenH / 2 - 500);
        AICar3.Position = new Vector2(_screenW / 2, _screenH / 2 - 600);
        _mapstartTime = OS.GetTicksMsec();
    }

    public void SetTiles()
    {
        for (int i = 0; i < TileMapParts.track.Count; i++)
        {
            SetTrackParts(TileMapParts.track[i]);
        }
    }

    public void AddTrackPart()
    {
        int lastPart = TileMapParts.track[TileMapParts.track.Count -1];
        int partToAdd = (int)GD.RandRange(0, TileMapParts.mapCounterparts[lastPart].Length);
        TileMapParts.track.Add(TileMapParts.mapCounterparts[lastPart][partToAdd]);

        SetTrackParts(TileMapParts.track[TileMapParts.track.Count -1]);
    }

    public void SetTrackParts(int part)
    {
        int partLength = TileMapParts.mapParts[part].Length;
        
        CurrentMapHeight -= partLength;
        for (int i = 0; i < partLength; i++)
        {
            for (int j = 0; j < TileMapParts.mapParts[part][i].Length; j++)
            {
                TileMapTrack.SetCell(j, CurrentMapHeight + i, TileMapParts.mapParts[part][i][j]);
                if (TileMapParts.mapParts[part][i][j] == 3)
                {
                    SetDecorations(j, CurrentMapHeight + i);
                }
            }
        }
    }

    public void SetDecorations(int x, int y)
    {
        TileMapDecor.SetCell(x, y, _decorationTiles[(int)GD.RandRange(0, _decorationTiles.Length)]);
    }

    public override void _Process(float delta)
    {
        if (PlayerCar.Position.y - _screenH < CurrentMapHeight * _tileSize || AICar.Position.y - _screenH < CurrentMapHeight * _tileSize)
        {
            AddTrackPart();
        }
    }

    private void _OnPlayTimerTimeout()
    {
        var timenow = OS.GetTicksMsec();
        var elapsed = timenow - _mapstartTime;
        uint test = 60000;
        if (elapsed < test)
        {
            var elapsedSecs = (elapsed / 1000);
            GD.Print("Playtime: " + elapsedSecs + " seconds");
        }
        else 
        {
            var elapsedMins = ((float)elapsed / 1000) / 60;
            GD.Print(String.Format("Playtime: {0:0.00} minutes", elapsedMins));
        }
    }
}
