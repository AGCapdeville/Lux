using System;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Vector3 = UnityEngine.Vector3;

public class Board
{
    public int SpaceWidth {get; set;}
    public int SpaceLength {get; set;}
    public int NumberOfRows {get; set;}
    public int NumberOfColumns {get; set;}

    public List<GameObject> MovementTiles {get; set;} // The displayed movement for heros...
    public HashSet<Space> MovementGridSpaces {get; set;} // Hero movement spaces

    private GameObject GameBoardObject {get; set;}
    private List<Entity> _Entities {get; set;}
    private List<Hero> _Heroes {get; set;}
    private List<Enemy> _Enemies  {get; set;}

    // FOR MAP DATA
    public Dictionary<Vector3, Space> MapData {get; set;}
    private List<(int, int)> Blocked_Locations;

    // FOR DRAWING line for pathing 
    private LineRenderer lineRenderer;
    private GameObject lineObj;

    public Board(int numberOfRows, int numberOfColumns, int spaceWidth, int spaceLength)
    {
        // Make board:
        SpaceWidth = spaceWidth;
        SpaceLength = spaceLength;
        NumberOfRows = numberOfRows;
        NumberOfColumns = numberOfColumns;

        GameBoardObject = new GameObject("Board");

        MapData = GenerateBoardSpaces(NumberOfRows, NumberOfColumns, SpaceWidth, SpaceLength);
        
        _Heroes = new List<Hero>();
        _Entities = new List<Entity>();
    }

    /// <summary>Creates the Board</summary>
    public Dictionary<Vector3, Space> GenerateBoardSpaces(int Rows, int Columns, int SpaceWidth, int SpaceLength) {
        Dictionary<Vector3, Space> spaces = new Dictionary<Vector3, Space>();
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                spaces[new Vector3(row, 0f, col)] =
                    new Space(
                        GameBoardObject,
                        new Vector3(row * SpaceWidth, 0f, col * SpaceLength),
                        new Vector3(SpaceWidth, 1f, SpaceLength)
                    );
            }   
        }
        DrawGridLines(Rows, Columns);
        return spaces;
    }

    /// <summary>Add Entity to Board</summary>
    public void AddHero(Hero h) 
    {
        _Heroes.Add(h); 
        MapData[h.Position].entity = h;
    }

    public void AddEntity(Entity e) 
    {
        _Entities.Add(e);
        MapData[e.Position].entity = e;
    }

    // /// <summary>Add Board as child to GameObject</summary>
    // public void SetParent(GameObject go) {
    //     GameBoardObject.transform.parent = go.transform;
    // }

    /// <summary>Draws grid lines onto the board.</summary>
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
        // MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();
        // MeshFilter filter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer renderer = GameBoardObject.AddComponent<MeshRenderer>();
        MeshFilter filter = GameBoardObject.AddComponent<MeshFilter>();

        // Assign the mesh to the filter
        filter.mesh = mesh;

        // Create a new material for the grid lines
        Material gridMaterial = new Material(Shader.Find("Unlit/Color"));
        Color gridColor = Color.white; // Color of the grid lines
        gridMaterial.color = gridColor;

        // Assign the material to the renderer
        renderer.material = gridMaterial;
    }

    public Entity GetEntity(Vector3 position, string type) {
        foreach (Entity e in _Entities)
        {
            if (e.Position == position && e.Type == type) {
                return e;
            }
        }
        return null;
    }

    public void DisplayHeroGrid(Hero hero) {

        if (hero.MovementRange == null) {
            hero.MovementRange = GetMovementRange(hero); // Fetch HashSet<Spaces> to create the Tiles...
        }
        hero.MovementTiles = DisplayMovementRange(hero); // Store MovementTiles to be destroyed later...
    }

    public void HideHeroGrid(Hero hero) {

        foreach (GameObject tile in hero.MovementTiles)
        {
            GameObject.Destroy(tile);
        } 
        hero.MovementTiles = new List<GameObject>();
    }



// -------------------------------------- TO BE IMPLEMENTED --------------------------------------

    // DRAW PATH for pathing

        // Then fetch character position [x,z]
        // Node playerNode = new Node(0, 0);
        // playerNode.Type = SpaceEnum.Player;

        // Then we need to get the cube we clicked on position [x,z]
        // Node endNode = new Node((int)transform.position.x, (int)transform.position.z);

        // Calculate shortest path to clicked location
        // Pathing playerPathing = new Pathing(Map_Data, Distance);
        // playerPathing.ClosedList[(playerNode.Position["x"], playerNode.Position["y"])] = playerNode;

        // !!PATHING!!
        // List<Node> path = playerPathing.FindPath(playerNode, endNode);

        // DrawPath(path);


        // public static Dictionary<(int, int), PathNode> GenerateMap(List<(int, int)> blocked, int distance) {
        //     var genMap = new Dictionary<(int, int), PathNode>();
        //     for (int x = 0; x < 5; x++)
        //     {
        //         for (int y = 0; y < 5; y++)
        //         {
        //             var node = new PathNode(x * distance, y * distance);
        //             if (blocked.Contains((x, y)))
        //             {
        //                 node.Type = blocked;
        //             }
        //             genMap[(x * distance, y * distance)] = node;
        //         }
        //     }
        //     return genMap;
        // }


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


    // // Takes in the current state of the game board, and retuns what spaces (tiles) 
    // //   sprites should be rendered for move range of the player.
    public List<GameObject> DisplayMovementRange(Hero hero) {

        GameObject tilePrefab = Resources.Load<GameObject>("MovementTile");
        MovementTiles = new List<GameObject>();
        
        // Light up board with MovementGridSpaces, and create planes in those spaces...
        foreach (Space space in hero.MovementRange)
        {
            GameObject movementTile = GameObject.Instantiate(tilePrefab, Vector3.zero, Quaternion.identity);
            movementTile.transform.position = new Vector3(
                space.Position.x,
                space.Position.y + 1,
                space.Position.z
            );

            MovementTiles.Add(movementTile);
            // DestroyAfterDelay(movementTile, 1f);
            movementTile.transform.position = space.Position;
            // movementTile.transform.SetParent(space.transform); // Set the parent to make it a child of this GameObject
        }
        return MovementTiles;
    }

    public void HideMovementRange() {
        // UpdateMovementRange(board);
        // Light up board with MovementGridSpaces, and create planes in those spaces...
        foreach (GameObject tile in MovementTiles)
        {
            GameObject.Destroy(tile);
        } 
        MovementTiles = new List<GameObject>();
    }

    // Gets the spaces which the player can potentally move to and returns them.
    public HashSet<Space> GetMovementRange(Hero hero) {

        // Find all positions for movement range -------------------------
        HashSet<Vector3> rangeSet = new HashSet<Vector3>{hero.Position};

        for(int i = 0; i < hero.Movement; i++){
            HashSet<Vector3> tempSet =  new HashSet<Vector3>();

            foreach(Vector3 pos in rangeSet){
                if(IsValidSpace(MapData, new Vector3(pos.x + SpaceWidth, pos.y, pos.z)))
                    tempSet.Add(new Vector3(pos.x + SpaceWidth, pos.y, pos.z));
                
                if(IsValidSpace(MapData, new Vector3(pos.x - SpaceWidth, pos.y, pos.z)))
                    tempSet.Add(new Vector3(pos.x - SpaceWidth, pos.y, pos.z));
                
                if(IsValidSpace(MapData, new Vector3(pos.x, pos.y, pos.z  + SpaceLength)))
                    tempSet.Add(new Vector3(pos.x, pos.y, pos.z  + SpaceLength));

                if(IsValidSpace(MapData, new Vector3(pos.x, pos.y, pos.z - SpaceLength)))
                    tempSet.Add(new Vector3(pos.x, pos.y, pos.z - SpaceLength));
            }

            rangeSet.UnionWith(tempSet);
        }

        // Find all spaces from board that are from the range set ------
        HashSet<Space> MovementSpaces = new HashSet<Space>(); 
        foreach (Vector3 position in rangeSet) {
            MovementSpaces.Add(MapData[position]);
        }
        
        return MovementSpaces;
    }

    // Deprecated
    // private bool IsValidSpace(List<List<Space>> grid, Vector3 vectorToCheck ){

    //     List<Vector3> spaces = new List<Vector3>();

    //     foreach(List<Space> row in grid){
    //         foreach(Space space in row){
    //             spaces.Add(space.Object.transform.position);
    //         }
    //     }
        
    //     return spaces.Contains(vectorToCheck);
    // }

    private bool IsValidSpace(Dictionary<Vector3, Space> map, Vector3 position) {
        foreach (var key in map.Keys) {
            if (key == position) {
                // Checks to make sure the position passed, exists on the board (x, z)
                return true;
            }
        }
        return false;
    }

    





}
