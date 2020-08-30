using Godot;
using System;
using System.Collections.Generic;

public class TileMapParts : TileMap
{
    /*public static int[][] widestStraight = {
        new int[] {3, 5, 9, 5, 16, 0, 0, 2, 0, 0, 0, 19, 6, 8, 5, 4, },
        new int[] {5, 3, 5, 6, 16, 0, 0, 0, 0, 0, 0, 19, 4, 3, 4, 3, },
        new int[] {4, 3, 9, 6, 16, 0, 0, 0, 0, 0, 0, 19, 3, 9, 4, 3, },
        new int[] {3, 8, 3, 4, 16, 0, 0, 0, 1, 0, 1, 19, 4, 3, 3, 3, },
        new int[] {3, 3, 9, 3, 16, 1, 0, 2, 0, 0, 0, 19, 5, 4, 9, 5, },
        new int[] {4, 9, 3, 6, 16, 0, 1, 1, 0, 0, 0, 19, 5, 3, 3, 3, },
        new int[] {3, 3, 10, 4, 16, 0, 0, 0, 0, 0, 2, 19, 3, 10, 4, 3, },
        new int[] {10, 4, 3, 3, 16, 2, 1, 0, 0, 0, 0, 19, 6, 5, 8, 4, },
        new int[] {4, 5, 5, 5, 16, 0, 0, 0, 0, 1, 1, 19, 3, 3, 3, 10, },
        new int[] {3, 10, 3, 5, 16, 0, 0, 0, 1, 0, 1, 19, 6, 3, 9, 3, },
    };*/
    public static int[][] widestStraight = {
        new int[] {3, 3, 3, 3, 16, 0, 0, 2, 0, 0, 0, 19, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 16, 0, 0, 0, 0, 0, 0, 19, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 16, 0, 0, 0, 0, 0, 0, 19, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 16, 0, 0, 0, 1, 0, 1, 19, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 16, 1, 0, 2, 0, 0, 0, 19, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 16, 0, 1, 1, 0, 0, 0, 19, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 16, 0, 0, 0, 0, 0, 2, 19, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 16, 2, 1, 0, 0, 0, 0, 19, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 16, 0, 0, 0, 0, 1, 1, 19, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 16, 0, 0, 0, 1, 0, 1, 19, 3, 3, 3, 3, },
    };
    public static int[][] wideStraight = {
        new int[] {3,  3,  3,  3, 3, 16, 0, 2, 0, 0, 19, 3, 3, 3, 3, 3, },
        new int[] {3,  3,  3,  3, 3, 16, 0, 0, 0, 0, 19, 3, 3, 3, 3, 3, },
        new int[] {3,  3,  3,  3, 3, 16, 0, 0, 0, 0, 19, 3, 3, 3, 3, 3, },
        new int[] {3,  3,  3,  3, 3, 16, 0, 0, 1, 0, 19, 3, 3, 3, 3, 3, },
        new int[] {3,  3,  3,  3, 3, 16, 0, 2, 0, 0, 19, 3, 3, 3, 3, 3, },
        new int[] {3,  3,  3,  3, 3, 16, 1, 1, 0, 0, 19, 3, 3, 3, 3, 3, },
        new int[] {3,  3,  3,  3, 3, 16, 0, 0, 0, 0, 19, 3, 3, 3, 3, 3, },
        new int[] {3,  3,  3,  3, 3, 16, 1, 0, 0, 0, 19, 3, 3, 3, 3, 3, },
        new int[] {3,  3,  3,  3, 3, 16, 0, 0, 0, 1, 19, 3, 3, 3, 3, 3, },
        new int[] {3,  3,  3,  3, 3, 16, 0, 0, 1, 0, 19, 3, 3, 3, 3, 3, },
    };

    public static int[][] narrowStraight = {
        new int[] {3, 3, 3, 3, 3, 3, 16, 2, 0, 19, 3, 3, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 3, 3, 16, 0, 0, 19, 3, 3, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 3, 3, 16, 0, 0, 19, 3, 3, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 3, 3, 16, 0, 1, 19, 3, 3, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 3, 3, 16, 2, 0, 19, 3, 3, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 3, 3, 16, 1, 0, 19, 3, 3, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 3, 3, 16, 0, 0, 19, 3, 3, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 3, 3, 16, 0, 0, 19, 3, 3, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 3, 3, 16, 0, 0, 19, 3, 3, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 3, 3, 16, 0, 1, 19, 3, 3, 3, 3, 3, 3, },
    };

    public static int[][] narrowestStraight = {
        new int[] {3, 3, 3, 3, 3, 3, 3, 16, 19, 3, 3, 3, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 3, 3, 3, 16, 19, 3, 3, 3, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 3, 3, 3, 16, 19, 3, 3, 3, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 3, 3, 3, 16, 19, 3, 3, 3, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 3, 3, 3, 16, 19, 3, 3, 3, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 3, 3, 3, 16, 19, 3, 3, 3, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 3, 3, 3, 16, 19, 3, 3, 3, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 3, 3, 3, 16, 19, 3, 3, 3, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 3, 3, 3, 16, 19, 3, 3, 3, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 3, 3, 3, 16, 19, 3, 3, 3, 3, 3, 3, 3, },
    };

    public static int[][] narrowStraightRight = {
        new int[] { 3,  3,  3,  3,  3,  3,  3,  3,  3,  3, 16,  0,  0,  0, 19,  3, },
        new int[] { 3,  3,  3,  3,  3,  3,  3,  3,  3,  3, 16,  0,  0,  1, 19,  3, },
        new int[] { 3,  3,  3,  3,  3,  3,  3,  3,  3,  3, 16,  1,  1,  0, 19,  3, },
        new int[] { 3,  3,  3,  3,  3,  3,  3,  3,  3,  3, 16,  1,  0,  0, 19,  3, },
        new int[] { 3,  3,  3,  3,  3,  3,  3,  3,  3,  3, 16,  1,  0,  1, 19,  3, },
        new int[] { 3,  3,  3,  3,  3,  3,  3,  3,  3,  3, 16,  1,  1,  0, 19,  3, },
        new int[] { 3,  3,  3,  3,  3,  3,  3,  3,  3,  3, 16,  0,  1,  0, 19,  3, },
        new int[] { 3,  3,  3,  3,  3,  3,  3,  3,  3,  3, 16,  0,  1,  1, 19,  3, },
        new int[] { 3,  3,  3,  3,  3,  3,  3,  3,  3,  3, 16,  1,  1,  1, 19,  3, },
        new int[] { 3,  3,  3,  3,  3,  3,  3,  3,  3,  3, 16,  1,  1,  1, 19,  3, },
    };

    public static int[][] centerToRight = {
        new int[] { 3,  3,  3,  3,  3,  3,  3,  3,  3, 24, 25,  0,  0,  0, 36,  3, },
        new int[] { 3,  3,  3,  3,  3,  3,  3,  3,  3, 26,  1,  0,  0,  1, 37,  3, },
        new int[] { 3,  3,  3,  3,  3,  3,  3,  3,  3, 61,  1,  1,  1,  0, 38,  3, },
        new int[] { 3,  3,  3,  3,  3,  3,  3,  3, 59, 60,  1,  1,  0, 48, 47,  3, },
        new int[] { 3,  3,  3,  3,  3,  3,  3,  3, 58,  1,  1,  1,  0, 46,  3,  3, },
        new int[] { 3,  3,  3,  3,  3,  3,  3, 56, 57,  1,  1,  1, 45, 47,  3,  3, },
        new int[] { 3,  3,  3,  3,  3,  3,  3, 61,  0,  1,  0,  0, 49,  3,  3,  3, },
        new int[] { 3,  3,  3,  3,  3,  3, 59, 60,  0,  1,  1, 48, 47,  3,  3,  3, },
        new int[] { 3,  3,  3,  3,  3,  3, 58,  0,  1,  1,  0, 37,  3,  3,  3,  3, },
        new int[] { 3,  3,  3,  3,  3, 59, 60,  0,  1,  0,  1, 38,  3,  3,  3,  3, },
        new int[] { 3,  3,  3,  3,  3, 27,  0,  0,  1,  1, 35, 34,  3,  3,  3,  3, },
        new int[] { 3,  3,  3,  3,  3, 28,  0,  2,  1,  1, 19,  3,  3,  3,  3,  3, },
        new int[] { 3,  3,  3,  3,  3, 16,  0,  1,  2,  0, 19,  3,  3,  3,  3,  3, },
        new int[] { 3,  3,  3,  3,  3, 16,  0,  2,  0,  0, 19,  3,  3,  3,  3,  3, },

    };

    public static int[][] rightToCenter = {
        new int[] { 3,  3,  3,  3,  3, 41,  1,  1,  1,  1, 30, 29,  3,  3,  3,  3, },
        new int[] { 3,  3,  3,  3,  3, 52,  1,  1,  1,  1,  0, 31,  3,  3,  3,  3, },
        new int[] { 3,  3,  3,  3,  3, 50, 51,  0,  1,  0,  0, 64,  3,  3,  3,  3, },
        new int[] { 3,  3,  3,  3,  3,  3, 55,  1,  0,  0,  0, 30, 29,  3,  3,  3, },
        new int[] { 3,  3,  3,  3,  3,  3, 53, 54,  0,  1,  0,  1, 31,  3,  3,  3, },
        new int[] { 3,  3,  3,  3,  3,  3,  3, 42,  1,  0,  0,  1, 32,  3,  3,  3, },
        new int[] { 3,  3,  3,  3,  3,  3,  3, 43,  1,  1,  1,  0, 33,  3,  3,  3, },
        new int[] { 3,  3,  3,  3,  3,  3,  3, 39, 40,  0,  1,  0, 30, 29,  3,  3, },
        new int[] { 3,  3,  3,  3,  3,  3,  3,  3, 41,  1,  0,  1,  1, 31,  3,  3, },
        new int[] { 3,  3,  3,  3,  3,  3,  3,  3, 55,  0,  0,  1,  1, 64,  3,  3, },
        new int[] { 3,  3,  3,  3,  3,  3,  3,  3, 53, 54,  1,  0,  0, 63, 65,  3, },
        new int[] { 3,  3,  3,  3,  3,  3,  3,  3,  3, 52,  0,  0,  0,  1, 32,  3, },
        new int[] { 3,  3,  3,  3,  3,  3,  3,  3,  3, 39, 40,  1,  1,  0, 33,  3, },

    };

    public static int[][] centerToLeft = {
        new int[] { 3, 41,  1,  1,  1, 30, 29,  3,  3,  3,  3,  3,  3,  3,  3,  3, },
        new int[] { 3, 52,  1,  1,  1,  1, 31,  3,  3,  3,  3,  3,  3,  3,  3,  3, },
        new int[] { 3, 50, 51,  0,  1,  0, 64,  3,  3,  3,  3,  3,  3,  3,  3,  3, },
        new int[] { 3,  3, 55,  1,  0,  0, 66, 62,  3,  3,  3,  3,  3,  3,  3,  3, },
        new int[] { 3,  3, 39, 40,  0,  1,  0, 67,  3,  3,  3,  3,  3,  3,  3,  3, },
        new int[] { 3,  3,  3, 41,  1,  0,  0, 66, 62,  3,  3,  3,  3,  3,  3,  3, },
        new int[] { 3,  3,  3, 52,  1,  1,  1,  0, 64,  3,  3,  3,  3,  3,  3,  3, },
        new int[] { 3,  3,  3, 39, 40,  0,  1,  0, 30, 29,  3,  3,  3,  3,  3,  3, },
        new int[] { 3,  3,  3,  6, 41,  1,  0,  1,  1, 31,  3,  3,  3,  3,  3,  3, },
        new int[] { 3,  3,  3,  3, 55,  0,  0,  1,  1, 64,  3,  3,  3,  3,  3,  3, },
        new int[] { 3,  3,  3,  3, 39, 40,  1,  0,  0, 63, 65,  3,  3,  3,  3,  3, },
        new int[] { 3,  3,  3,  3,  3, 16,  0,  0,  0,  1, 32,  3,  3,  3,  3,  3, },
        new int[] { 3,  3,  3,  3,  3, 16,  2,  1,  1,  0, 33,  4,  3,  3,  3,  3, },

    };

    public static int[][] leftToCenter = {
        new int[] { 3,  3,  3,  3,  3, 16,  0,  2,  0,  0, 36,  3,  3,  3,  3,  3, },
        new int[] { 3,  3,  3,  3,  3, 16,  1,  0,  0,  0, 37,  3,  3,  3,  3,  3, },
        new int[] { 3,  3,  3,  3, 24, 25,  1,  0,  0,  1, 38,  3,  3,  3,  3,  3, },
        new int[] { 3,  3,  3,  3, 26,  0,  1,  1,  1, 45, 47,  3,  3,  3,  3,  3, },
        new int[] { 3,  3,  3,  3, 27,  1,  1,  1,  0, 46,  3,  3,  3,  3,  3,  3, },
        new int[] { 3,  3,  3,  3, 28,  1,  1,  1, 48, 47,  3,  3,  3,  3,  3,  3, },
        new int[] { 3,  3,  3, 56, 57,  1,  1,  1, 46,  3,  3,  3,  3,  3,  3,  3, },
        new int[] { 3,  3,  3, 61,  0,  0,  0, 45, 47,  3,  3,  3,  3,  3,  3,  3, },
        new int[] { 3,  3, 59, 60,  0,  0,  0, 49,  3,  3,  3,  3,  3,  3,  3,  3, },
        new int[] { 3,  3, 58,  0,  1,  1, 48, 47,  3,  3,  3,  3,  3,  3,  3,  3, },
        new int[] { 3, 59, 60,  0,  1,  0, 37,  3,  3,  3,  3,  3,  3,  3,  3,  3, },
        new int[] { 3, 27,  0,  0,  1,  1, 38,  3,  3,  3,  3,  3,  3,  3,  3,  3, },
        new int[] { 3, 28,  0,  2,  1, 35, 34,  3,  3,  3,  3,  3,  3,  3,  3,  3, },
    };


    public static int[][] narrowStraightLeft = {
        new int[] { 3, 16,  1,  0,  0, 19,  3,  3,  3,  3,  3,  3,  3,  3,  3,  3, },
        new int[] { 3, 16,  1,  0,  1, 19,  3,  3,  3,  3,  3,  3,  3,  3,  3,  3, },
        new int[] { 3, 16,  1,  1,  0, 19,  3,  3,  3,  3,  3,  3,  3,  3,  3,  3, },
        new int[] { 3, 16,  0,  1,  0, 19,  3,  3,  3,  3,  3,  3,  3,  3,  3,  3, },
        new int[] { 3, 16,  0,  1,  1, 19,  3,  3,  3,  3,  3,  3,  3,  3,  3,  3, },
        new int[] { 3, 16,  1,  1,  1, 19,  3,  3,  3,  3,  3,  3,  3,  3,  3,  3, },
        new int[] { 3, 16,  1,  1,  1, 19,  3,  3,  3,  3,  3,  3,  3,  3,  3,  3, },
        new int[] { 3, 16,  1,  0,  0, 19,  3,  3,  3,  3,  3,  3,  3,  3,  3,  3, },
        new int[] { 3, 16,  1,  0,  1, 19,  3,  3,  3,  3,  3,  3,  3,  3,  3,  3, },
        new int[] { 3, 16,  1,  1,  0, 19,  3,  3,  3,  3,  3,  3,  3,  3,  3,  3, },
    };

    public static int[][] widestToWide = {
        new int[] {3, 3, 3, 3, 24, 25, 0, 2, 0, 0, 30, 29, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 26,  0, 0, 0, 0, 0,  0, 31, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 27,  0, 0, 0, 0, 0,  2, 32, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 28,  0, 0, 0, 1, 0,  0, 33, 3, 3, 3, 3, },
    };

    public static int[][] wideToNarrow = {
        new int[] {3, 3, 3, 3, 3, 24, 25, 2, 0, 30, 29, 3, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 3, 26,  0, 0, 0,  0, 31, 3, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 3, 27,  0, 0, 0,  2, 32, 3, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 3, 28,  0, 0, 1,  0, 33, 3, 3, 3, 3, 3, },
    };

    public static int[][] narrowToNarrowest = {
        new int[] {3, 3, 3, 3, 3, 3, 24, 25, 30, 29, 3, 3, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 3, 3, 26,  0,  0, 31, 3, 3, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 3, 3, 27,  0,  2, 32, 3, 3, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 3, 3, 28,  0,  0, 33, 3, 3, 3, 3, 3, 3, },
    };

    public static int[][] narrowestToNarrow = {
        new int[] {3, 3, 3, 3, 3, 3, 41,  0,  0, 36, 3, 3, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 3, 3, 42,  0,  0, 37, 3, 3, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 3, 3, 43,  0,  0, 38, 3, 3, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 3, 3, 39, 40, 35, 34, 3, 3, 3, 3, 3, 3, },
    };

    public static int[][] narrowToWide = {
        new int[] {3, 3, 3, 3, 3, 41,  0, 0, 0,  0, 36, 3, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 3, 42,  0, 0, 0,  0, 37, 3, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 3, 43,  0, 0, 0,  0, 38, 3, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 3, 39, 40, 1, 1, 35, 34, 3, 3, 3, 3, 3, },
    };

    public static int[][] wideToWidest = {
        new int[] {3, 3, 3, 3, 41,  0, 0, 0, 0, 0,  0, 36, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 42,  0, 0, 0, 0, 0,  0, 37, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 43,  0, 0, 0, 0, 0,  0, 38, 3, 3, 3, 3, },
        new int[] {3, 3, 3, 3, 39, 40, 2, 0, 0, 1, 35, 34, 3, 3, 3, 3, },
    };

    public static int[][] twoLaneStart = {
        new int[] { 1, 1, 1, 2, 0, 0, 0, 34, 41, 0, 0, 0, 9, 1, 1, 1 },
        new int[] { 1, 1, 1, 2, 0, 0, 0, 34, 41, 0, 0, 0, 9, 1, 1, 1 },
        new int[] { 1, 1, 1, 2, 0, 0, 0, 36, 42, 0, 0, 0, 9, 1, 1, 1 },
        new int[] { 1, 1, 1, 2, 0, 0, 0, 38, 43, 0, 0, 0, 9, 1, 1, 1 },
    };

    public static int[][] twoLaneStraight = {
        new int[] { 1, 1, 1, 2, 0, 0, 0, 34, 41, 0, 0, 0, 9, 1, 1, 1 },
        new int[] { 1, 1, 1, 2, 0, 0, 0, 34, 41, 0, 0, 0, 9, 1, 1, 1 },
        new int[] { 1, 1, 1, 2, 0, 0, 0, 34, 41, 0, 0, 0, 9, 1, 1, 1 },
        new int[] { 1, 1, 1, 2, 0, 0, 0, 34, 41, 0, 0, 0, 9, 1, 1, 1 },
        new int[] { 1, 1, 1, 2, 0, 0, 0, 34, 41, 0, 0, 0, 9, 1, 1, 1 },
        new int[] { 1, 1, 1, 2, 0, 0, 0, 34, 41, 0, 0, 0, 9, 1, 1, 1 },
        new int[] { 1, 1, 1, 2, 0, 0, 0, 34, 41, 0, 0, 0, 9, 1, 1, 1 },
        new int[] { 1, 1, 1, 2, 0, 0, 0, 34, 41, 0, 0, 0, 9, 1, 1, 1 },
        new int[] { 1, 1, 1, 2, 0, 0, 0, 34, 41, 0, 0, 0, 9, 1, 1, 1 },
        new int[] { 1, 1, 1, 2, 0, 0, 0, 34, 41, 0, 0, 0, 9, 1, 1, 1 },
    };

    public static int[][] twoLaneEnd = {
        new int[] { 1, 1, 1, 2, 0, 0, 0, 30, 39, 0, 0, 0, 9, 1, 1, 1 },
        new int[] { 1, 1, 1, 2, 0, 0, 0, 32, 40, 0, 0, 0, 9, 1, 1, 1 },
        new int[] { 1, 1, 1, 2, 0, 0, 0, 34, 41, 0, 0, 0, 9, 1, 1, 1 },
        new int[] { 1, 1, 1, 2, 0, 0, 0, 34, 41, 0, 0, 0, 9, 1, 1, 1 },
    };

    public static int[][] twoLaneNarrowLeft = {
        new int[] { 3,  2, 0, 8, 45, 53, 55, 26, 0, 0, 0, 0, 12, 1, 1, 1,  },
        new int[] { 3,  2, 0, 11, 56, 45, 53, 10, 0, 0,  0, 18, 19, 1, 1, 1 },
        new int[] { 3, 25, 0, 16, 17, 45, 14, 15, 0, 0,  0, 24,  1, 1, 1, 1 },
        new int[] { 3, 29, 0, 0, 23, 53, 22,  0, 0, 0,  0, 28,  1, 1, 1, 1 },
        new int[] { 3, 13, 0, 0, 47, 45, 49,  0, 0, 0,  0, 12,  1, 1, 1, 1 },
        new int[] { 3, 20, 21, 0, 50, 51, 52,  0, 0, 0, 18, 19,  1, 1, 1, 1 }
    };

    public static int[][] rightTurn = {
        new int[] { }
    };

    public static List<int[][]> mapParts = new List<int[][]>
    {
        widestStraight,             //  0
        wideStraight,               //  1
        narrowStraight,             //  2
        narrowestStraight,          //  3
        widestToWide,               //  4
        wideToNarrow,               //  5
        narrowToNarrowest,          //  6
        narrowestToNarrow,          //  7
        narrowToWide,               //  8
        wideToWidest,               //  9
        twoLaneStart,               // 10
        twoLaneStraight,            // 11
        twoLaneEnd,                 // 12
        narrowStraightRight,        // 13
        narrowStraightLeft,         // 14
        centerToRight,              // 15
        rightToCenter,              // 16
        centerToLeft,               // 17
        leftToCenter,               // 18
        twoLaneNarrowLeft           // 19
    };

    public static int[][] mapCounterparts = {
        new int[] {0, 0, 0, 4, 4}, // 0
        new int[] {1, 1, 5, 9, 15, 17}, // 1
        new int[] {2, 2, 6, 8}, // 2
        new int[] {3, 3, 3, 7, 7}, // 3
        new int[] {1, 1, 5}, // 4
        new int[] {2, 2, 6}, // 5
        new int[] {3 }, // 6
        new int[] {2, 2, 2, 8}, // 7
        new int[] {1, 1, 9}, // 8
        new int[] {0}, // 9
        new int[] {0}, // 10
        new int[] {0}, // 11
        new int[] {0}, // 12
        new int[] {13, 13, 16}, // 13
        new int[] {14, 14, 18}, // 14
        new int[] {13}, // 15
        new int[] {1}, // 16
        new int[] {14}, // 17
        new int[] {1}, // 18
        new int[] {0}, // 19
    };
    /*
        0 - Widest *
        1 - Wide *
        2 - Narrow *
        3 - Narrowest *
        4 - Widest to Wide *
        5 - Wide to Narrow *
        6 - Narrow to Narrowest *
        7 - Narrowest to Narrow *
        8 - Narrow to Wide *
        9 - Wide to Widest *
       10 - Two Lane Start
       11 - Two Lane Straight
       12 - Two Lane End
       13 - Narrow Straight Right
       14 - Narrow Straight Left
       15 - Center to Right
    */

    public static List<int> track = new List<int> {
      0, 0, 0, 0, 0, 0, 4, 1, 1, 15
    };
}
