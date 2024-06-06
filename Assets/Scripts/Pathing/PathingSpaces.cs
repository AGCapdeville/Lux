using System;
using System.Collections.Generic;
using Scripts.Enums;

public class PathingSpaces
{
    private Dictionary<(int, int), Space> BoardSpaces;
    public Dictionary<(int, int), Space> OpenList { get; private set; }
    public Dictionary<(int, int), Space> ClosedList { get; private set; }
    private int Distance;

    public PathingSpaces(Dictionary<(int, int), Space> boardSpaces, int distance)
    {
        BoardSpaces = boardSpaces;
        Distance = distance;
        OpenList = new Dictionary<(int, int), Space>();
        ClosedList = new Dictionary<(int, int), Space>();
    }

    /// <summary> Resets pathing, and updates the board spaces.</summary>
    /// <param name="boardSpaces">The current board spaces</param>  
    public void ResetBoardSpaces(Dictionary<(int, int), Space> boardSpaces)
    {
        BoardSpaces = boardSpaces;
        OpenList = new Dictionary<(int, int), Space>();
        ClosedList = new Dictionary<(int, int), Space>();
    }

    public List<Space> FindPath(Space start, Space end)
    {
        Space current = start;
        while (current.Position.x != end.Position.x || current.Position.z != end.Position.z)
        {
            FindOpenPaths(current, end);
            var (key, space) = FindBestPath();
            OpenList.Remove(key);
            ClosedList[key] = space;
            current = space;
        }

        List<Space> path = new List<Space> { end };
        Space cursor = end;
        while (cursor.Position.x != start.Position.x || cursor.Position.z != start.Position.z)
        {
            cursor = ClosedList[((int)cursor.Position.x, (int)cursor.Position.z)].Prev;
            path.Add(cursor);
        }
        path.Reverse();
        return path;
    }

    private void FindOpenPaths(Space current, Space end)
    {
        var cardinalDirections = new List<Dictionary<string, int>>
        {
            new Dictionary<string, int> { { "x", 0 }, { "z", Distance } },
            new Dictionary<string, int> { { "x", Distance }, { "z", 0 } },
            new Dictionary<string, int> { { "x", 0 }, { "z", -Distance } },
            new Dictionary<string, int> { { "x", -Distance }, { "z", 0 } }
        };

        foreach (var direction in cardinalDirections)
        {
            int newX = (int)current.Position.x + (int)direction["x"];
            int newZ = (int)current.Position.z + (int)direction["z"];
            var newLocation = (newX, newZ);

            if (BoardSpaces.ContainsKey(newLocation) && !ClosedList.ContainsKey(newLocation))
            {
                Space neighbor = BoardSpaces[newLocation];

                if (neighbor.SpaceMarking != SpaceEnum.Block)
                {
                    int G = neighbor.Cost + current.G;
                    double H = Math.Sqrt(Math.Pow((int)end.Position.x - newX, 2) + Math.Pow((int)end.Position.z - newZ, 2));
                    double F = G + H;

                    if (OpenList.ContainsKey(newLocation))
                    {
                        if (F < OpenList[newLocation].F)
                        {
                            neighbor.G = G;
                            neighbor.H = H;
                            neighbor.F = F;
                            neighbor.Prev = current;
                            OpenList[newLocation] = neighbor;
                        }
                    }
                    else
                    {
                        neighbor.G = G;
                        neighbor.H = H;
                        neighbor.F = F;
                        neighbor.Prev = current;
                        OpenList[newLocation] = neighbor;
                    }
                }
            }
        }
    }

    private ((int, int), Space) FindBestPath()
    {
        double smallestF = double.PositiveInfinity;
        (int, int) key = (0, 0);
        Space shortestPath = null;

        foreach (var item in OpenList)
        {
            if (item.Value.F < smallestF)
            {
                smallestF = item.Value.F;
                key = item.Key;
                shortestPath = item.Value;
            }
        }

        return (key, shortestPath);
    }
}
