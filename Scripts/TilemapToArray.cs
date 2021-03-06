using Godot;
using System;

public class TilemapToArray : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    TileMap tiles;
    bool readout = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        tiles = (TileMap)GetNode("TileMap");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
    if (Input.IsActionPressed("ui_accept") && !readout)
    {
        readout = true;
        var xline = "new int[] {";
        for (int y = 0; y < 30; y++)
        {
            for (int x = 0; x < 30; x++)
            {
                int tileID = tiles.GetCell(x, y);
                if (tileID != -1) {
                    if (tileID < 10)
                    {
                        xline += " " + tiles.GetCell(x, y) + ", ";
                    }
                    else
                    {
                        xline += tiles.GetCell(x, y) + ", ";
                    }
                }
            }
            xline += "},";
            GD.Print(xline);
            xline = "new int[] {";
        }
    }    
  }
}
