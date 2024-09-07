using System;
using System.IO;
using System.Text.Json;
using UnityEngine;
using System.Collections.Generic;
using Vector3 = UnityEngine.Vector3;
using Scripts.Enums;

public class Board
{
    public int SpaceWidth = 10;
    public int SpaceLength = 10;

    public List<GameObject> MovementTiles { get; set; } // The displayed movement for heros...
    public HashSet<Space> MovementGridSpaces { get; set; } // Hero movement spaces

    private GameObject _GameBoardObject { get; set; }
    private List<Entity> _Entities { get; set; }

    // FOR MAP DATA
    public Dictionary<Vector3, Space> MapData { get; set; }
    // private List<(int, int)> _Blocked_Locations;

    // FOR DRAWING line for pathing 
    // private LineRenderer _lineRenderer;
    // private GameObject _lineObj;

    public Board()
    {
        // FETCH JSON MAP DATA:
        _GameBoardObject = new GameObject("Board");
        _Entities = new List<Entity>();

        string path = Path.Combine(Application.dataPath, "Maps/meadow.json");

        if (File.Exists(path))
        {
            Debug.Log($"Loaded, meadow.json at {path}");

            string json = File.ReadAllText(path);
            MapJSONData mapJSONData = JsonUtility.FromJson<MapJSONData>(json);

            MapData = GenerateBoardSpaces(mapJSONData.rows, mapJSONData.cols);

        }
        else
        {
            Debug.LogError($"Could not find meadow.json at {path}");
        }
        
    }

    // <summary>Creates the Board</summary>
    public Dictionary<Vector3, Space> GenerateBoardSpaces(int Rows, int Columns)
    {
        Dictionary<Vector3, Space> spaces = new Dictionary<Vector3, Space>();
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                spaces[new Vector3(row * SpaceWidth, 0f, col * SpaceLength)] =
                    new Space(
                        _GameBoardObject,
                        new Vector3(row * SpaceWidth, 0f, col * SpaceLength),
                        new Vector3(SpaceWidth, 1f, SpaceLength)
                    );
            }
        }
        DrawGridLines(Rows, Columns);
        return spaces;
    }

    public void AddEntity(Entity e)
    {
        _Entities.Add(e);
        MapData[e.Position].entity = e;
    }

    public void UpdateEnity(Entity e, Vector3 newPos)
    {
        if (_Entities.Contains(e))
        {
            MapData[e.Position].entity = null;
            MapData[newPos].entity = e;
        }
    }

    // <summary>Draws grid lines onto the board.</summary>
    public void DrawGridLines(int Rows, int Columns)
    {
        // Create a new mesh
        Mesh mesh = new Mesh();

        // Vertices array to hold positions of all vertices
        Vector3[] vertices = new Vector3[(Rows + 1) * (Columns + 1)];

        // Generate vertices
        for (int col = 0; col <= Columns; col++)
        {
            for (int row = 0; row <= Rows; row++)
            {
                vertices[col * (Rows + 1) + row] = new Vector3((float)(row * SpaceWidth - 0.5 * SpaceWidth), 0, (float)(col * SpaceLength - 0.5 * SpaceLength));
            }
        }

        // Assign vertices to the mesh
        mesh.vertices = vertices;

        // Generate horizontal lines
        int numHorizontalLines = Rows * (Columns + 1);
        int[] horizontalLineIndices = new int[numHorizontalLines * 2];
        int index = 0;
        for (int col = 0; col <= Columns; col++)
        {
            for (int row = 0; row < Rows; row++)
            {
                horizontalLineIndices[index++] = col * (Rows + 1) + row;
                horizontalLineIndices[index++] = col * (Rows + 1) + row + 1;
            }
        }

        // Generate vertical lines
        int numVerticalLines = (Rows + 1) * Columns;
        int[] verticalLineIndices = new int[numVerticalLines * 2];
        index = 0;
        for (int row = 0; row <= Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                verticalLineIndices[index++] = col * (Rows + 1) + row;
                verticalLineIndices[index++] = (col + 1) * (Rows + 1) + row;
            }
        }

        // Combine horizontal and vertical lines
        int[] allLineIndices = new int[numHorizontalLines * 2 + numVerticalLines * 2];
        horizontalLineIndices.CopyTo(allLineIndices, 0);
        verticalLineIndices.CopyTo(allLineIndices, numHorizontalLines * 2);

        // Assign the indices for the lines
        mesh.SetIndices(allLineIndices, MeshTopology.Lines, 0);

        // Create a mesh renderer and filter to render the mesh
        MeshRenderer renderer = _GameBoardObject.AddComponent<MeshRenderer>();
        MeshFilter filter = _GameBoardObject.AddComponent<MeshFilter>();

        // Assign the mesh to the filter
        filter.mesh = mesh;

        // Create a new material for the grid lines
        Material gridMaterial = new Material(Shader.Find("Unlit/Color"));
        Color gridColor = Color.white; // Color of the grid lines
        gridMaterial.color = gridColor;

        // Assign the material to the renderer
        renderer.material = gridMaterial;
    }

    public Entity GetEntity(Vector3 position, string type)
    {
        foreach (Entity e in _Entities)
        {
            if (e.Position == position && e.Type == type)
            {
                return e;
            }
        }
        return null;
    }

    public void DisplayHeroGrid(Hero hero)
    {
        if (hero.MovementRange == null)
        {
            hero.MovementRange = GetMovementRange(hero); // Fetch HashSet<Spaces> to create the Tiles...
        }
        DisplayMovementRange(hero); // Store MovementTiles to be destroyed later...
    }

    public Queue<Space> FindPath(Vector3 start, Vector3 end)
    {   

        Dictionary<Vector3, Space> OpenList = new Dictionary<Vector3, Space>();
        Dictionary<Vector3, Space> ClosedList  = new Dictionary<Vector3, Space>();

        Vector3 currentPos = start;
        while (currentPos != end)
        {
            FindOpenPaths(currentPos, end, OpenList, ClosedList);
            var (vector, space) = FindBestPath(OpenList); 
            OpenList.Remove(vector);
            ClosedList[vector] = space;
            currentPos = vector;
        }

        List<Space> path = new List<Space>{MapData[end]};
        Space cursor = MapData[end];

        while (cursor.SpaceGameObject.transform.position != start)
        {
            cursor = ClosedList[cursor.SpaceGameObject.transform.position].Prev;
            path.Add(cursor);
        }

        path.Reverse();
        return new Queue<Space>(path);
    }

    private void FindOpenPaths(Vector3 current, Vector3 end, Dictionary<Vector3, Space> Open, Dictionary<Vector3, Space> Closed)
    {
        var cardinalDirections = new List<Dictionary<string, int>>
        {
            new Dictionary<string, int> { { "x", 0 }, { "z", SpaceLength } },
            new Dictionary<string, int> { { "x", SpaceWidth }, { "z", 0 } },
            new Dictionary<string, int> { { "x", 0 }, { "z", -SpaceLength } },
            new Dictionary<string, int> { { "x", -SpaceWidth }, { "z", 0 } }
        }; 

        foreach (var direction in cardinalDirections)
        {
            float newX = current.x + direction["x"];
            float newZ = current.z + direction["z"];

            Vector3 newLocation = new Vector3(newX, 0, newZ);

            if (MapData.ContainsKey(newLocation) && !Closed.ContainsKey(newLocation))
            {
                Space neighbor = MapData[newLocation];

                if (neighbor.SpaceMarking != SpaceEnum.Block)
                {
                    int G = neighbor.Cost + MapData[current].G;
                    double H = Math.Sqrt(Math.Pow(MapData[end].Position.x - newX, 2) + Math.Pow(MapData[end].Position.z - newZ, 2));
                    double F = G + H;

                    if (Open.ContainsKey(newLocation))
                    {
                        if (F < Open[newLocation].F)
                        {
                            neighbor.G = G;
                            neighbor.H = H;
                            neighbor.F = F;
                            neighbor.Prev = MapData[current];
                            Open[newLocation] = neighbor;
                        }
                    }
                    else 
                    {
                        neighbor.G = G;
                        neighbor.H = H;
                        neighbor.F = F;
                        neighbor.Prev = MapData[current];
                        Open[newLocation] = neighbor;
                    }
                }
            }
        }
    }

    private (Vector3, Space) FindBestPath(Dictionary<Vector3, Space> Open)
    {
        double smallestF = double.PositiveInfinity;
        Vector3 smallestVector = new Vector3(0, 0, 0);
        Space smallestSpace = null;

        foreach (var item in Open)
        {
            if (item.Value.F < smallestF)
            {
                smallestF = item.Value.F;
                smallestVector = item.Key;
                smallestSpace = item.Value;
            }
        }

        return (smallestVector, smallestSpace);

    }

    public void DisplayMovementRange(Hero hero)
    {
        foreach (Space space in hero.MovementRange)
        {
            space.SpaceBehaviorScript.SetType(SpaceType.Movement);
        }
    }

    public void HideMovementRange(Hero hero)
    {
        foreach (Space space in hero.MovementRange)
        {
            space.SpaceBehaviorScript.SetType(SpaceType.Default);
        }
    }

    // Gets the spaces which the player can potentally move to and returns them.
    public HashSet<Space> GetMovementRange(Hero hero)
    {
        // Find all positions for movement range -------------------------
        HashSet<Vector3> rangeSet = new HashSet<Vector3> { hero.Position };

        for (int i = 0; i < hero.Movement; i++)
        {
            HashSet<Vector3> tempSet = new HashSet<Vector3>();

            foreach (Vector3 pos in rangeSet)
            {
                if (IsValidSpace(MapData, new Vector3(pos.x + SpaceWidth, pos.y, pos.z)))
                    tempSet.Add(new Vector3(pos.x + SpaceWidth, pos.y, pos.z));

                if (IsValidSpace(MapData, new Vector3(pos.x - SpaceWidth, pos.y, pos.z)))
                    tempSet.Add(new Vector3(pos.x - SpaceWidth, pos.y, pos.z));

                if (IsValidSpace(MapData, new Vector3(pos.x, pos.y, pos.z + SpaceLength)))
                    tempSet.Add(new Vector3(pos.x, pos.y, pos.z + SpaceLength));

                if (IsValidSpace(MapData, new Vector3(pos.x, pos.y, pos.z - SpaceLength)))
                    tempSet.Add(new Vector3(pos.x, pos.y, pos.z - SpaceLength));
            }

            rangeSet.UnionWith(tempSet);
        }

        // Find all spaces from board that are from the range set ------
        HashSet<Space> MovementSpaces = new HashSet<Space>();
        foreach (Vector3 position in rangeSet)
        {
            if (position != hero.Position)
            {
                MovementSpaces.Add(MapData[position]);
            }
            // Debug.Log("p:" + position.x);
        }

        return MovementSpaces;
    }

    private bool IsValidSpace(Dictionary<Vector3, Space> map, Vector3 position)
    {
        foreach (Vector3 key in map.Keys)
        {
            if (key == position)
            {
                return true;
            }
        }
        return false;
    }

}




// ------------------- OLD STUFF ----------------------------
    
    // -------------------------------------- TO BE IMPLEMENTED --------------------------------------

    // !!PATHING!!
    // List<Node> path = playerPathing.FindPath(playerNode, endNode);
    // DrawPath(path);
    // private LineRenderer lineRenderer;

    // void DrawPath(List<Node> path)
    // {
    //     if (lineRenderer == null)
    //     {
    //         GameObject lineObj = new GameObject("PathLine");
    //         lineRenderer = lineObj.AddComponent<LineRenderer>();

    //         // Configure the LineRenderer
    //         lineRenderer.startWidth = 0.2f;
    //         lineRenderer.endWidth = 0.2f;
    //         lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Use a default material
    //         lineRenderer.positionCount = path.Count;
    //         lineRenderer.useWorldSpace = true;
    //     }

    //     for (int i = 0; i < path.Count; i++)
    //     {
    //         Vector3 position = new Vector3(path[i].Position["x"], 0, path[i].Position["y"]);
    //         lineRenderer.SetPosition(i, position);
    //     }
    // }

    // -------------- Unit Movement ---------------------------------------------
