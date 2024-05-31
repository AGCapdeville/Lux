using UnityEngine;
using System.Collections.Generic;

public enum Direction
{
    North,
    South,
    East,
    West
}

public class DirectionMap
{
    private static readonly Dictionary<Direction, Vector3> directionMap = new Dictionary<Direction, Vector3>
    {
        { Direction.North, new Vector3(0, 0, 1) },
        { Direction.South, new Vector3(0, 0, -1) },
        { Direction.East, new Vector3(1, 0, 0) },
        { Direction.West, new Vector3(-1, 0, 0) }
    };

    public static Vector3 GetVector(Direction direction)
    {
        return directionMap[direction];
    }
}