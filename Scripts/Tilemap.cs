using Godot;
using System;

public class Tilemap : Node2D
{
    private int _screenH;
    private int _screenW;
    private int _tileSize = 64;
    TileMap tm;
    Camera2D cam;
    KinematicBody2D PlayerCar;
    KinematicBody2D AICar;
    
    public int CurrentMapHeight = 0;

    private uint _mapstartTime;
    Timer pt;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _screenW = (int)GetViewport().Size.x;
        _screenH = (int)GetViewport().Size.y;
        tm = (TileMap)GetNode("TileMap");
        pt = (Timer)GetNode("PlayTimer");
        pt.Connect("timeout", this, "_OnPlayTimerTimeout");
        SetTiles();
        cam = (Camera2D)GetNode("PlayerCar/Camera2D");
        cam.EditorDrawDragMargin = true;
        PlayerCar = (KinematicBody2D)GetNode("PlayerCar");
        AICar = (KinematicBody2D)GetNode("AICar1");
        PlayerCar.Position = new Vector2(_screenW / 2, _screenH / 2 - 400);
        AICar.Position = new Vector2(_screenW / 2 - 200, _screenH / 2 - 400);
        _mapstartTime = OS.GetTicksMsec();
    }

    public void SetTiles()
    {
        for (int i = 0; i < TileMapParts.track.Count; i++)
        {
            SetTileParts(TileMapParts.track[i], i);
        }
    }

    public void AddTrackPart()
    {
        int last = TileMapParts.track[TileMapParts.track.Count -1];
        int toAdd = (int)GD.RandRange(0, TileMapParts.mapCounterparts[last].Length);
        TileMapParts.track.Add(TileMapParts.mapCounterparts[last][toAdd]);

        SetTileParts(TileMapParts.track[TileMapParts.track.Count -1], 1);
        /*if (TileMapParts.track.Count > 5) 
        {
            TileMapParts.track.RemoveAt(0);
        }*/
    }

    public void SetTileParts(int part, int height)
    {
        CurrentMapHeight -= TileMapParts.mapParts[part].Length;
        for (int i = 0; i < TileMapParts.mapParts[part].Length; i++)
        {
            for (int j = 0; j < TileMapParts.mapParts[part][i].Length; j++)
            {
                tm.SetCell(j, CurrentMapHeight + i, TileMapParts.mapParts[part][i][j]);
            }
        }
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
