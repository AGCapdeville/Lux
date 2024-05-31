using System;
using System.Collections.Generic;

public enum SpaceEnum
{
    Empty = 0,
    Block = 1,
    Player = 2,
    Enemy = 3
    // Add more mappings as needed
}

public class PathNode
{
    public Dictionary<string, int> Position { get; set; }
    public int G { get; set; }
    public double H { get; set; }
    public double F { get; set; }
    public PathNode Prev { get; set; }
    public SpaceEnum Type { get; set; }
    public int Cost { get; set; }

    public PathNode(int x, int y)
    {
        Position = new Dictionary<string, int> { { "x", x }, { "y", y } };
        G = 0;
        H = 0;
        F = 0;
        Prev = null;
        Type = SpaceEnum.Empty;
        Cost = 1;
    }
}
