using Godot;
using System;
using System.Collections.Generic;

public class TileMapParts : TileMap
{
    public static int[][] mapPart1 = {
        new int[] { 1, 1, 1, 1, 2, 0, 0, 0, 0, 0, 0, 9, 1, 1, 1, 1 },
        new int[] { 1, 1, 1, 1, 2, 0, 0, 0, 0, 0, 0, 9, 1, 1, 1, 1 },
        new int[] { 1, 1, 1, 1, 2, 0, 0, 0, 0, 0, 0, 9, 1, 1, 1, 1 },
        new int[] { 1, 1, 1, 1, 2, 0, 0, 0, 0, 0, 0, 9, 1, 1, 1, 1 },
        new int[] { 1, 1, 1, 1, 2, 0, 0, 0, 0, 0, 0, 9, 1, 1, 1, 1 },
        new int[] { 1, 1, 1, 1, 2, 0, 0, 0, 0, 0, 0, 9, 1, 1, 1, 1 },
        new int[] { 1, 1, 1, 1, 2, 0, 0, 0, 0, 0, 0, 9, 1, 1, 1, 1 },
        new int[] { 1, 1, 1, 1, 2, 0, 0, 0, 0, 0, 0, 9, 1, 1, 1, 1 },
        new int[] { 1, 1, 1, 1, 2, 0, 0, 0, 0, 0, 0, 9, 1, 1, 1, 1 },
        new int[] { 1, 1, 1, 1, 2, 0, 0, 0, 0, 0, 0, 9, 1, 1, 1, 1 },
    };

    public static int[][] mapPart2 = {
        new int[] { 1, 1, 1, 1,  1,  2, 0, 0, 0, 0,  9, 1, 1, 1, 1, 1 },
        new int[] { 1, 1, 1, 1,  1,  2, 0, 0, 0, 0,  9, 1, 1, 1, 1, 1 },
        new int[] { 1, 1, 1, 1,  1,  2, 0, 0, 0, 0,  9, 1, 1, 1, 1, 1 },
        new int[] { 1, 1, 1, 1,  1,  2, 0, 0, 0, 0,  9, 1, 1, 1, 1, 1 },
        new int[] { 1, 1, 1, 1,  1,  2, 0, 0, 0, 0,  9, 1, 1, 1, 1, 1 },
        new int[] { 1, 1, 1, 1,  1,  2, 0, 0, 0, 0,  9, 1, 1, 1, 1, 1 },
        new int[] { 1, 1, 1, 1, 14, 15, 0, 0, 0, 0, 16, 17, 1, 1, 1, 1 },
        new int[] { 1, 1, 1, 1, 22,  0, 0, 0, 0, 0,  0, 23, 1, 1, 1, 1 },
        new int[] { 1, 1, 1, 1, 26,  0, 0, 0, 0, 0,  0, 27, 1, 1, 1, 1 },
        new int[] { 1, 1, 1, 1, 10,  0, 0, 0, 0, 0,  0, 11, 1, 1, 1, 1 },
    };

    public static List<int[][]> mapParts = new List<int[][]> { mapPart1, mapPart2 };

    public static int[] track = {
        1, 0, 0, 0, 0
    };

    public override void _Ready()
    {

    }
}
