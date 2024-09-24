using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using Vector3 = UnityEngine.Vector3;
using Scripts.Enums;
using Newtonsoft.Json;


public class Board
{
    public int SpaceWidth = 10;
    public int SpaceLength = 10;

    public List<GameObject> MovementTiles { get; set; } // The displayed movement for heros...
    public HashSet<Space> MovementGridSpaces { get; set; } // Hero movement spaces

    private GameObject _GameBoardObject { get; set; }
    private GameObject _GameBoardWallContainer { get; set; }
    private List<Unit> _Units { get; set; }

    // FOR MAP DATA
    public Dictionary<Vector3, Space> MapData { get; set; }

    public Dictionary<Vector3, string> WallData { get; set; }

    // private List<(int, int)> _Blocked_Locations;

    // FOR DRAWING line for pathing 
    // private LineRenderer _lineRenderer;
    // private GameObject _lineObj;

    public Board()
    {
        // FETCH JSON MAP DATA:
        _GameBoardObject = new GameObject("Board");
        _GameBoardWallContainer = new GameObject("Walls");
        _Units = new List<Unit>();

        string path = Path.Combine(Application.dataPath, "Maps/ruin.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            JSONMapData jsonMapData = JsonConvert.DeserializeObject<JSONMapData>(json);

            MapData = LoadGameBoardFromJSON(jsonMapData);
            WallData = FindWalls(MapData);
            SpawnWalls(WallData);

            // temp solution for gameobjects:
            addStoneColumn(new Vector3(10,0,10));

        }
        else
        {
            Debug.LogError($"Could not find meadow.json at {path}");
        }
        
    }

    private void addStoneColumn(Vector3 location) {

        GameObject SpawnedStoneColumn = GameObject.Instantiate (
            Resources.Load<GameObject>("Prefabs/StoneColumn"),
            new Vector3(location.x, location.y, location.z),
            Quaternion.identity
        );

        MapData[location].BoardGameObject = SpawnedStoneColumn;
        MapData[location].State = SpaceState.Block;
    }

    private Dictionary<Vector3, string> FindWalls(Dictionary<Vector3, Space> mapData)
    {
        Dictionary<Vector3, string> found_walls = new Dictionary<Vector3, string>();

        foreach (KeyValuePair<Vector3, Space> entry in mapData)
        {
            Vector3 position = entry.Key;   // Key (Vector3)
            Space spaceData = entry.Value;  // Value (Space)

            Console.WriteLine($"Position: {position}, Space Data: {spaceData}");

            if (spaceData.Walls["north"] == "stone-wall") {
                found_walls[new Vector3(position.x, position.y, position.z + 5)] = "stone-wall";
            }
            if (spaceData.Walls["east"] == "stone-wall") {
                found_walls[new Vector3(position.x + 5, position.y, position.z)] = "stone-wall";
            }
            if (spaceData.Walls["south"] == "stone-wall") {
                found_walls[new Vector3(position.x, position.y, position.z - 5)] = "stone-wall";
            }
            if (spaceData.Walls["west"] == "stone-wall") {
                found_walls[new Vector3(position.x - 5, position.y, position.z)] = "stone-wall";
            }
        }

        return found_walls;
    }

    private void SpawnWalls(Dictionary<Vector3, string> wallData) {
        foreach (KeyValuePair<Vector3, string> wall in wallData) {
            if (Math.Abs(wall.Key.x) / 5 % 2 != 0) {
                GameObject SpawnedWall = GameObject.Instantiate (
                    Resources.Load<GameObject>("Prefabs/Wall"),
                    new Vector3(wall.Key.x, wall.Key.y, wall.Key.z),
                    Quaternion.Euler(0, 90, 0) // rotate to align with the z-axis
                );
                SpawnedWall.transform.parent = _GameBoardWallContainer.transform;
            } else {
                GameObject SpawnedWall = GameObject.Instantiate (
                    Resources.Load<GameObject>("Prefabs/Wall"),
                    new Vector3(wall.Key.x, wall.Key.y, wall.Key.z),
                    Quaternion.identity // rotate to align with the z-axis
                );
                SpawnedWall.transform.parent = _GameBoardWallContainer.transform;
            }
        }
    }

    // <summary>Creates the Board</summary>
    public Dictionary<Vector3, Space> LoadGameBoardFromJSON(JSONMapData jsonMapData)
    {

        Dictionary<Vector3, Space> spaces = new Dictionary<Vector3, Space>();
        for (int row = 0; row < int.Parse(jsonMapData.rows); row++)
        {
            for (int col = 0; col < int.Parse(jsonMapData.cols); col++)
            {
                Dictionary<string, string> walls = new Dictionary<string, string>();
                JSONTileData tile = jsonMapData.map[row + ",0," + col];

                walls["north"] = tile.north;
                walls["east"] = tile.east;
                walls["south"] = tile.south;
                walls["west"] = tile.west;

                spaces[new Vector3(row * SpaceWidth, 0f, col * SpaceLength)] =
                    new Space(
                        _GameBoardObject,
                        new Vector3(row * SpaceWidth, 0f, col * SpaceLength),
                        walls,
                        tile.terrain
                    );
            }
        }
        DrawGridLines(int.Parse(jsonMapData.rows), int.Parse(jsonMapData.cols));
        return spaces;
    }

    public void AddUnit(Unit newUnit, Vector3 location)
    {
        _Units.Add(newUnit);
        MapData[location].unit = newUnit;
    }

    public void UpdateUnit(Unit unit, Vector3 newLocation)
    {
        if (_Units.Contains(unit))
        {
            MapData[unit.Position].unit = null;
            MapData[newLocation].unit = unit;
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

    public Unit GetUnit(Vector3 position)
    {
        foreach (Unit unit in _Units)
        {
            if (unit.Position == position)
            {
                return unit;
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
            new Dictionary<string, int> { { "x", 0 }, { "z", SpaceLength }, {"wall_x", 0}, {"wall_z", SpaceLength/2} },
            new Dictionary<string, int> { { "x", SpaceWidth }, { "z", 0 }, {"wall_x", SpaceLength/2}, {"wall_z", 0} },
            new Dictionary<string, int> { { "x", 0 }, { "z", -SpaceLength }, {"wall_x", 0}, {"wall_z", -SpaceLength/2} },
            new Dictionary<string, int> { { "x", -SpaceWidth }, { "z", 0 }, {"wall_x", -SpaceLength/2}, {"wall_z", 0} }
        }; 

        foreach (var direction in cardinalDirections)
        {
            float newX = current.x + direction["x"];
            float newZ = current.z + direction["z"];

            float wallX = current.x + direction["wall_x"];
            float wallZ = current.z + direction["wall_z"];

            Vector3 newLocation = new Vector3(newX, 0, newZ);
            Vector3 wallLocation = new Vector3(wallX, 0, wallZ);

            if (MapData.ContainsKey(newLocation) && !Closed.ContainsKey(newLocation) && !WallData.ContainsKey(wallLocation))
            {
                Space neighbor = MapData[newLocation];

                if (neighbor.State != SpaceState.Block)
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
                Vector3 map_pos = new Vector3(pos.x, pos.y, pos.z);
                if (MapData[map_pos].Walls["north"] == "no-wall") {
                    if (IsValidSpace(MapData, new Vector3(pos.x, pos.y, pos.z + SpaceLength))) {
                        tempSet.Add(new Vector3(pos.x, pos.y, pos.z + SpaceLength));
                    }
                }
                if (MapData[map_pos].Walls["east"] == "no-wall") {
                    if (IsValidSpace(MapData, new Vector3(pos.x + SpaceWidth, pos.y, pos.z))){
                        tempSet.Add(new Vector3(pos.x + SpaceWidth, pos.y, pos.z));
                    }
                }
                if (MapData[map_pos].Walls["south"] == "no-wall") {
                    if (IsValidSpace(MapData, new Vector3(pos.x, pos.y, pos.z - SpaceLength))) {
                        tempSet.Add(new Vector3(pos.x, pos.y, pos.z - SpaceLength));
                    }
                }
                if (MapData[map_pos].Walls["west"] == "no-wall") {
                    if (IsValidSpace(MapData, new Vector3(pos.x - SpaceWidth, pos.y, pos.z))){
                        tempSet.Add(new Vector3(pos.x - SpaceWidth, pos.y, pos.z));
                    }
                }
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
            if (key == position) // does position being checked exist in map?
            {
                // Need to check to see if a wall exists in this direction?
                return true; // tile exists so return can travel to.
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
