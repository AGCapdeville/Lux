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
    public GameObject MovementTile {get;}

    private GameObject GameBoardObject {get; set;}
    private List<Entity> Entities;

    // Constructor
    public Board(int numberOfRows, int numberOfColumns, int spaceWidth, int spaceLength)
    {
        SpaceWidth = spaceWidth;
        SpaceLength = spaceLength;
        NumberOfRows = numberOfRows;
        NumberOfColumns = numberOfColumns;

        MovementTile = Resources.Load<GameObject>("MovementTile");
        GameBoardObject = new GameObject("Board");

        Grid = GenerateGrid();
        Entities = new List<Entity>();
    }

    public void AddEntity(Entity entity) 
    {
        Entities.Add(entity);
    }

    public void SetParent(GameObject go) {
        GameBoardObject.transform.parent = go.transform;
    }

    List<List<Space>> GenerateGrid() {
        List<List<Space>> spaces = new List<List<Space>>();
        for (int row = 0; row < NumberOfRows; row++)
        {
            spaces.Add(new List<Space>());
            for (int col = 0; col < NumberOfColumns; col++)
            {
                spaces[row].Add(
                    new Space(
                        new Vector3(row * SpaceWidth, 0f, col * SpaceLength), 
                        new Vector3(SpaceWidth, 1f, SpaceLength)
                    )
                );
            }   
        }
        GenerateGridLines();
        return spaces;
    }

    void GenerateGridLines()
    {
        // Create a new mesh
        Mesh mesh = new Mesh();

        // Vertices array to hold positions of all vertices
        Vector3[] vertices = new Vector3[(NumberOfRows + 1) * (NumberOfColumns + 1)];

        // Generate vertices
        for (int col = 0; col <= NumberOfColumns; col++)
        {
            for (int row = 0; row <= NumberOfRows; row++)
            {
                vertices[col * (NumberOfRows + 1) + row] = new Vector3((float)(row * SpaceWidth - 0.5 * SpaceWidth), 0, (float)(col * SpaceLength - 0.5 * SpaceLength));
            }
        }

        // Assign vertices to the mesh
        mesh.vertices = vertices;

        // Generate horizontal lines
        int numHorizontalLines = NumberOfRows * (NumberOfColumns + 1);
        int[] horizontalLineIndices = new int[numHorizontalLines * 2];
        int index = 0;
        for (int col = 0; col <= NumberOfColumns; col++)
        {
            for (int row = 0; row < NumberOfRows; row++)
            {
                horizontalLineIndices[index++] = col * (NumberOfRows + 1) + row;
                horizontalLineIndices[index++] = col * (NumberOfRows + 1) + row + 1;
            }
        }

        // Generate vertical lines
        int numVerticalLines = (NumberOfRows + 1) * NumberOfColumns;
        int[] verticalLineIndices = new int[numVerticalLines * 2];
        index = 0;
        for (int row = 0; row <= NumberOfRows; row++)
        {
            for (int col = 0; col < NumberOfColumns; col++)
            {
                verticalLineIndices[index++] = col * (NumberOfRows + 1) + row;
                verticalLineIndices[index++] = (col + 1) * (NumberOfRows + 1) + row;
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


    // -------------- Unit Movement ---------------------------------------------


    // Takes in the current state of the game board, and retuns what spaces (tiles) 
    //   sprites should be rendered for move range of the player.
    public void DisplayMovementRange(Board board) {
        UpdateMovementRange(board);
        MovementTiles = new List<GameObject>();
        // Light up board with MovementGridSpaces, and create planes in those spaces...
        foreach (Space space in MovementGridSpaces)
        {
            GameObject movementTile = GameObject.Instantiate(board.MovementTile, Vector3.zero, Quaternion.identity);
            movementTile.transform.position = new Vector3(
                space.Object.transform.position.x,
                space.Object.transform.position.y + 1,
                space.Object.transform.position.z
            );

            MovementTiles.Add(movementTile);
            // DestroyAfterDelay(movementTile, 1f);
            // movementTile.transform.SetParent(space.transform); // Set the parent to make it a child of this GameObject
        }
    }
    public void HideMovementRange(Board board) {
        UpdateMovementRange(board);
        // Light up board with MovementGridSpaces, and create planes in those spaces...
        foreach (GameObject tile in MovementTiles)
        {
            GameObject.Destroy(tile);
        }
        MovementTiles = new List<GameObject>();
    }
    public void UpdateMovementRange(Board board) {

        HashSet<Vector3> rangeSet = new HashSet<Vector3>
        {
            //Starting Postion of the player
            Piece.transform.position
        };

        for(int i = 0; i < Movement; i++){

            HashSet<Vector3> tempSet =  new HashSet<Vector3>();

            foreach(Vector3 pos in rangeSet){

                if(IsValidSpace(board.Grid, new Vector3(pos.x + board.SpaceWidth, pos.y, pos.z)))
                    tempSet.Add(new Vector3(pos.x + board.SpaceWidth, pos.y, pos.z));
                
                if(IsValidSpace(board.Grid, new Vector3(pos.x - board.SpaceWidth, pos.y, pos.z)))
                    tempSet.Add(new Vector3(pos.x - board.SpaceWidth, pos.y, pos.z));
                
                if(IsValidSpace(board.Grid, new Vector3(pos.x, pos.y, pos.z  + board.SpaceLength)))
                    tempSet.Add(new Vector3(pos.x, pos.y, pos.z  + board.SpaceLength));
                
                if(IsValidSpace(board.Grid, new Vector3(pos.x, pos.y, pos.z - board.SpaceLength)))
                    tempSet.Add(new Vector3(pos.x, pos.y, pos.z - board.SpaceLength));

            }

            rangeSet.UnionWith(tempSet);
        }

        HashSet<Space> MovementSpaces = new HashSet<Space>(); 
        bool found = false;

        foreach(Vector3 position in rangeSet){

            foreach(List<Space> row in board.Grid)
            {
                foreach(Space space in row){
                    if(space.Object.transform.position == position){
                         MovementSpaces.Add(space);
                         found = true;
                         break;
                    }
                }
                if(found){
                    found = false;
                    break;
                }
            }
        }
        MovementGridSpaces = MovementSpaces;
    }
    // Gets the spaces which the player can potentally move to and returns them.
    public HashSet<Space> GetMovementRange(Board board) {

        HashSet<Vector3> rangeSet = new HashSet<Vector3>
        {
            //Starting Postion of the player
            Piece.transform.position
        };

        for(int i = 0; i < Movement; i++){

            HashSet<Vector3> tempSet =  new HashSet<Vector3>();

            foreach(Vector3 pos in rangeSet){

                if(IsValidSpace(board.Grid, new Vector3(pos.x + board.SpaceWidth, pos.y, pos.z)))
                    tempSet.Add(new Vector3(pos.x + board.SpaceWidth, pos.y, pos.z));
                
                if(IsValidSpace(board.Grid, new Vector3(pos.x - board.SpaceWidth, pos.y, pos.z)))
                    tempSet.Add(new Vector3(pos.x - board.SpaceWidth, pos.y, pos.z));
                
                if(IsValidSpace(board.Grid, new Vector3(pos.x, pos.y, pos.z  + board.SpaceLength)))
                    tempSet.Add(new Vector3(pos.x, pos.y, pos.z  + board.SpaceLength));
                
                if(IsValidSpace(board.Grid, new Vector3(pos.x, pos.y, pos.z - board.SpaceLength)))
                    tempSet.Add(new Vector3(pos.x, pos.y, pos.z - board.SpaceLength));

            }

            rangeSet.UnionWith(tempSet);
        }

        HashSet<Space> MovementSpaces = new HashSet<Space>(); 
        bool found = false;

        foreach(Vector3 position in rangeSet){

            foreach(List<Space> row in board.Grid)
            {
                foreach(Space space in row){
                    if(space.Object.transform.position == position){
                        MovementSpaces.Add(space);
                        found = true;
                        break;
                    }
                }
                if(found){
                    found = false;
                    break;
                }
            }
        }
        
        return MovementSpaces;
    }
    private bool IsValidSpace(List<List<Space>> grid, Vector3 vectorToCheck ){

        List<Vector3> spaces = new List<Vector3>();

        foreach(List<Space> row in grid){
            foreach(Space space in row){
                spaces.Add(space.Object.transform.position);
            }
        }
        
        return spaces.Contains(vectorToCheck);
    }

    





}
