using System;
using System.Collections.Generic;

public class Pathing
{
    private Dictionary<(int, int), Node> MapData;
    public Dictionary<(int, int), Node> OpenList { get; private set; }
    public Dictionary<(int, int), Node> ClosedList { get; private set; }
    private int Distance;

    public Pathing(Dictionary<(int, int), Node> mapData, int distance)
    {
        MapData = mapData;
        OpenList = new Dictionary<(int, int), Node>();
        ClosedList = new Dictionary<(int, int), Node>();
        Distance = distance;
    }

    public List<Node> FindPath(Node start, Node end)
    {
        Node currentNode = start;
        while (currentNode.Position["x"] != end.Position["x"] || currentNode.Position["y"] != end.Position["y"])
        {
            FindOpenPaths(currentNode, end);
            var (key, node) = FindBestPath();
            OpenList.Remove(key);
            ClosedList[key] = node;
            currentNode = node;
        }

        List<Node> path = new List<Node> { end };
        Node cursor = end;
        while (cursor.Position["x"] != start.Position["x"] || cursor.Position["y"] != start.Position["y"])
        {
            cursor = ClosedList[(cursor.Position["x"], cursor.Position["y"])].Prev;
            path.Add(cursor);
        }
        path.Reverse();
        return path;
    }

    private void FindOpenPaths(Node currentNode, Node endNode)
    {
        var cardinalDirections = new List<Dictionary<string, int>>
        {
            new Dictionary<string, int> { { "x", 0 }, { "y", Distance } },
            new Dictionary<string, int> { { "x", Distance }, { "y", 0 } },
            new Dictionary<string, int> { { "x", 0 }, { "y", -Distance } },
            new Dictionary<string, int> { { "x", -Distance }, { "y", 0 } }
        };

        foreach (var direction in cardinalDirections)
        {
            int newX = currentNode.Position["x"] + direction["x"];
            int newY = currentNode.Position["y"] + direction["y"];
            var newLocation = (newX, newY);

            if (MapData.ContainsKey(newLocation) && !ClosedList.ContainsKey(newLocation))
            {
                Node neighbor = MapData[newLocation];

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

    private (ValueTuple<int, int>, Node) FindBestPath()
    {
        double smallestF = double.PositiveInfinity;
        (int, int) smallestKey = (0, 0);
        Node smallestNode = null;

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
