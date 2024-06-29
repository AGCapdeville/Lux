using System;
using System.Collections.Generic;

public class Pathing
{
    private Dictionary<(int, int), PathNode> MapData;
    public Dictionary<(int, int), PathNode> OpenList { get; private set; }
    public Dictionary<(int, int), PathNode> ClosedList { get; private set; }
    private int _Distance;

    public Pathing(Dictionary<(int, int), PathNode> mapData, int distance)
    {
        MapData = mapData;
        OpenList = new Dictionary<(int, int), PathNode>();
        ClosedList = new Dictionary<(int, int), PathNode>();
        _Distance = distance;
    }

    public List<PathNode> FindPath(PathNode start, PathNode end)
    {
        PathNode currentNode = start;
        while (currentNode.Position["x"] != end.Position["x"] || currentNode.Position["y"] != end.Position["y"])
        {
            FindOpenPaths(currentNode, end);
            var (key, node) = FindBestPath();
            OpenList.Remove(key);
            ClosedList[key] = node;
            currentNode = node;
        }

        List<PathNode> path = new List<PathNode> { end };
        PathNode cursor = end;
        while (cursor.Position["x"] != start.Position["x"] || cursor.Position["y"] != start.Position["y"])
        {
            cursor = ClosedList[(cursor.Position["x"], cursor.Position["y"])].Prev;
            path.Add(cursor);
        }
        path.Reverse();
        return path;
    }

    private void FindOpenPaths(PathNode currentNode, PathNode endNode)
    {
        var cardinalDirections = new List<Dictionary<string, int>>
        {
            new Dictionary<string, int> { { "x", 0 }, { "y", _Distance } },
            new Dictionary<string, int> { { "x", _Distance }, { "y", 0 } },
            new Dictionary<string, int> { { "x", 0 }, { "y", -_Distance } },
            new Dictionary<string, int> { { "x", -_Distance }, { "y", 0 } }
        };

        foreach (var direction in cardinalDirections)
        {
            int newX = currentNode.Position["x"] + direction["x"];
            int newY = currentNode.Position["y"] + direction["y"];
            var newLocation = (newX, newY);

            if (MapData.ContainsKey(newLocation) && !ClosedList.ContainsKey(newLocation))
            {
                PathNode neighbor = MapData[newLocation];

                if (neighbor.Type != SpaceEnum.Block)
                {
                    int G = neighbor.Cost + currentNode.G;
                    double H = Math.Sqrt(Math.Pow(endNode.Position["x"] - newX, 2) + Math.Pow(endNode.Position["y"] - newY, 2));
                    double F = G + H;

                    if (OpenList.ContainsKey(newLocation))
                    {
                        if (F < OpenList[newLocation].F)
                        {
                            neighbor.G = G;
                            neighbor.H = H;
                            neighbor.F = F;
                            neighbor.Prev = currentNode;
                            OpenList[newLocation] = neighbor;
                        }
                    }
                    else
                    {
                        neighbor.G = G;
                        neighbor.H = H;
                        neighbor.F = F;
                        neighbor.Prev = currentNode;
                        OpenList[newLocation] = neighbor;
                    }
                }
            }
        }
    }

    private (ValueTuple<int, int>, PathNode) FindBestPath()
    {
        double smallestF = double.PositiveInfinity;
        (int, int) smallestKey = (0, 0);
        PathNode smallestNode = null;

        foreach (var item in OpenList)
        {
            if (item.Value.F < smallestF)
            {
                smallestF = item.Value.F;
                smallestKey = item.Key;
                smallestNode = item.Value;
            }
        }

        return (smallestKey, smallestNode);
    }
}
