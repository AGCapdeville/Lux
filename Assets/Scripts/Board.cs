using System;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Board
{
    // Properties
    public List<List<Space>> Grid {get; set;}

    public int SpaceWidth {get; set;}
    public int SpaceLength {get; set;}
    public int NumberOfRows {get; set;}
    public int NumberOfColumns {get; set;}

    public List<GameObject> MovementTiles {get; set;}
    public HashSet<Space> MovementGridSpaces {get; set;}

    public GameObject MovementTile {get;}

    private GameObject GameBoardObject {get; set;}
    private List<Entity> Entities;

    // FOR MAP DATA
    public Dictionary<(int, int), Space> MapData {get; set;}
    private List<(int, int)> Blocked_Locations;

    // FOR DRAWING line for pathing 
    private LineRenderer lineRenderer;
    private GameObject lineObj;

    // ENUMS
    // public SpaceEnum blocked = SpaceEnum.Block; // not used yet...


    // Constructor
    public Board(int numberOfRows, int numberOfColumns, int spaceWidth, int spaceLength)
    {
        SpaceWidth = spaceWidth;
        SpaceLength = spaceLength;
        NumberOfRows = numberOfRows;
        NumberOfColumns = numberOfColumns;

        MovementTile = Resources.Load<GameObject>("MovementTile");
        GameBoardObject = new GameObject("Board");

        Entities = new List<Entity>();

        // Empty blocked list
        // List<(int, int)> Blocked_Locations = new List<(int, int)>();

        MapData = GenerateBoardSpaces(NumberOfRows, NumberOfColumns, SpaceWidth, SpaceLength);

    }

    public void AddEntity(Entity entity) 
    {
        Entities.Add(entity);
        // How should we add units to the board??? Through map_data and or Space obj??
        // Map_Data[(entity.x, entity.y)] = entity;
    }

    public void SetParent(GameObject go) {
        GameBoardObject.transform.parent = go.transform;
    }

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

    public Dictionary<(int, int), Space> GenerateBoardSpaces(int Rows, int Columns, int SpaceWidth, int SpaceLength) {
        Dictionary<(int, int), Space> spaces = new Dictionary<(int, int), Space>();
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                spaces[(row, col)] =
                    new Space(
                        new Vector3(row * SpaceWidth, 0f, col * SpaceLength), 
                        new Vector3(SpaceWidth, 1f, SpaceLength)
                    );
            }   
        }
        DrawGridLines(Rows, Columns);
        return spaces;
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


    // Takes in the current state of the game board, and retuns what spaces (tiles) 
    //   sprites should be rendered for move range of the player.
    // public void DisplayMovementRange(Board board) {
    //     // UpdateMovementRange(board);
    //     MovementTiles = new List<GameObject>();
    //     // Light up board with MovementGridSpaces, and create planes in those spaces...
    //     foreach (Space space in MovementGridSpaces)
    //     {
    //         GameObject movementTile = GameObject.Instantiate(board.MovementTile, Vector3.zero, Quaternion.identity);
    //         movementTile.transform.position = new Vector3(
    //             space.Object.transform.position.x,
    //             space.Object.transform.position.y + 1,
    //             space.Object.transform.position.z
    //         );

    //         MovementTiles.Add(movementTile);
    //         // DestroyAfterDelay(movementTile, 1f);
    //         // movementTile.transform.SetParent(space.transform); // Set the parent to make it a child of this GameObject
    //     }
    // }

    // public void HideMovementRange(Board board) {
    //     // UpdateMovementRange(board);
    //     // Light up board with MovementGridSpaces, and create planes in those spaces...
    //     foreach (GameObject tile in MovementTiles)
    //     {
    //         GameObject.Destroy(tile);
    //     }
    //     MovementTiles = new List<GameObject>();
    // }
    
    // Needs to be updated with selected Piece to update range for:
    // public void UpdateMovementRange(Board board) {

        // HashSet<Vector3> rangeSet = new HashSet<Vector3>
        // {
        //     //Starting Postion of the player
        //     Piece.transform.position
        // };

        // for(int i = 0; i < Movement; i++){

        //     HashSet<Vector3> tempSet =  new HashSet<Vector3>();

        //     foreach(Vector3 pos in rangeSet){

        //         if(IsValidSpace(board.Grid, new Vector3(pos.x + board.SpaceWidth, pos.y, pos.z)))
        //             tempSet.Add(new Vector3(pos.x + board.SpaceWidth, pos.y, pos.z));
                
        //         if(IsValidSpace(board.Grid, new Vector3(pos.x - board.SpaceWidth, pos.y, pos.z)))
        //             tempSet.Add(new Vector3(pos.x - board.SpaceWidth, pos.y, pos.z));
                
        //         if(IsValidSpace(board.Grid, new Vector3(pos.x, pos.y, pos.z  + board.SpaceLength)))
        //             tempSet.Add(new Vector3(pos.x, pos.y, pos.z  + board.SpaceLength));
                
        //         if(IsValidSpace(board.Grid, new Vector3(pos.x, pos.y, pos.z - board.SpaceLength)))
        //             tempSet.Add(new Vector3(pos.x, pos.y, pos.z - board.SpaceLength));

        //     }

        //     rangeSet.UnionWith(tempSet);
        // }

        // HashSet<Space> MovementSpaces = new HashSet<Space>(); 
        // bool found = false;

        // foreach(Vector3 position in rangeSet){

        //     foreach(List<Space> row in board.Grid)
        //     {
        //         foreach(Space space in row){
        //             if(space.Object.transform.position == position){
        //                  MovementSpaces.Add(space);
        //                  found = true;
        //                  break;
        //             }
        //         }
        //         if(found){
        //             found = false;
        //             break;
        //         }
        //     }
        // }
        // MovementGridSpaces = MovementSpaces;
    // }
    // Gets the spaces which the player can potentally move to and returns them.
    // public HashSet<Space> GetMovementRange(Board board) {

    //     // HashSet<Vector3> rangeSet = new HashSet<Vector3>
    //     // {
    //     //     //Starting Postion of the player
    //     //     Piece.transform.position
    //     // };

    //     // for(int i = 0; i < Movement; i++){

    //     //     HashSet<Vector3> tempSet =  new HashSet<Vector3>();

    //     //     foreach(Vector3 pos in rangeSet){

    //     //         if(IsValidSpace(board.Grid, new Vector3(pos.x + board.SpaceWidth, pos.y, pos.z)))
    //     //             tempSet.Add(new Vector3(pos.x + board.SpaceWidth, pos.y, pos.z));
                
    //     //         if(IsValidSpace(board.Grid, new Vector3(pos.x - board.SpaceWidth, pos.y, pos.z)))
    //     //             tempSet.Add(new Vector3(pos.x - board.SpaceWidth, pos.y, pos.z));
                
    //     //         if(IsValidSpace(board.Grid, new Vector3(pos.x, pos.y, pos.z  + board.SpaceLength)))
    //     //             tempSet.Add(new Vector3(pos.x, pos.y, pos.z  + board.SpaceLength));
                
    //     //         if(IsValidSpace(board.Grid, new Vector3(pos.x, pos.y, pos.z - board.SpaceLength)))
    //     //             tempSet.Add(new Vector3(pos.x, pos.y, pos.z - board.SpaceLength));

    //     //     }

    //     //     rangeSet.UnionWith(tempSet);
    //     // }

    //     // HashSet<Space> MovementSpaces = new HashSet<Space>(); 
    //     // bool found = false;

    //     // foreach(Vector3 position in rangeSet){

    //     //     foreach(List<Space> row in board.Grid)
    //     //     {
    //     //         foreach(Space space in row){
    //     //             if(space.Object.transform.position == position){
    //     //                 MovementSpaces.Add(space);
    //     //                 found = true;
    //     //                 break;
    //     //             }
    //     //         }
    //     //         if(found){
    //     //             found = false;
    //     //             break;
    //     //         }
    //     //     }
    //     // }
        
    //     // return MovementSpaces;
    // }
    // private bool IsValidSpace(List<List<Space>> grid, Vector3 vectorToCheck ){

    //     List<Vector3> spaces = new List<Vector3>();

    //     foreach(List<Space> row in grid){
    //         foreach(Space space in row){
    //             spaces.Add(space.Object.transform.position);
    //         }
    //     }
        
    //     return spaces.Contains(vectorToCheck);
    // }

    





}
