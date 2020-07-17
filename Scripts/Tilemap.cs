using Godot;
using System;

public class Tilemap : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private int _screenH;
    private int _screenW;
    private int _tileSize = 32;
    TileMap tm; 

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _screenW = (int)GetViewport().Size.x;
        _screenH = (int)GetViewport().Size.y;
        tm = (TileMap)GetNode("TileMap");
        SetTiles();
    }

    async public void SetTiles()
    {
        for (int i = 0; i <= _screenW / _tileSize; i++)
        {
            for (int j = 1; j <= _screenH / _tileSize; j++)
            {
                tm.SetCell(i, j, 0);
                await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
            }
        }
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {

    }
}
