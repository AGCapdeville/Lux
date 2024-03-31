using System;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Board
{
    // Properties
    public List<List<GameObject>> Grid {get; set;}
    public int SpaceWidth {get; set;}
    public int SpaceHeight {get; set;}
    public int NumberOfRows {get; set;}
    public int NumberOfColumns {get; set;}

    public GameObject MovementTile {get;}


    private GameObject GameBoardObject {get; set;}

    // Constructor
    public Board(int numberOfRows, int numberOfColumns, int spaceWidth, int spaceHeight)
    {
        SpaceWidth = spaceWidth;
        SpaceHeight = spaceHeight;
        NumberOfRows = numberOfRows;
        NumberOfColumns = numberOfColumns;

        MovementTile = Resources.Load<GameObject>("MovementTile");
        GameBoardObject = new GameObject("Board");

        Grid = GenerateGrid();
    }

    public void SetParent(GameObject go) {
        GameBoardObject.transform.parent = go.transform;
    }

    List<List<GameObject>> GenerateGrid() {
        List<List<GameObject>> spaces = new List<List<GameObject>>();
        for (int row = 0; row < NumberOfRows; row++)
        {
            spaces.Add(new List<GameObject>());
            for (int col = 0; col < NumberOfColumns; col++)
            {
                GameObject space = new GameObject("space ["  + (row * SpaceWidth) + "," + (col * SpaceHeight) + "]");
                space.transform.localScale = new Vector3(SpaceWidth, 1f, SpaceHeight);
                space.transform.position = new Vector3(row * SpaceWidth, 0f, col * SpaceHeight);
                spaces[row].Add(space);
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
                vertices[col * (NumberOfRows + 1) + row] = new Vector3((float)(row * SpaceWidth - 0.5 * SpaceWidth), 0, (float)(col * SpaceHeight - 0.5 * SpaceHeight));
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

    // Need to move this to a debug class or something:
    // void spawnText(string text, float xpos, float ypos, float zpos) {
    //     GameObject loadedTextPrefab = Resources.Load<GameObject>("DebugTextPrefab");
    //     GameObject newText = GameObject.Instantiate(loadedTextPrefab, new Vector3(xpos, ypos, zpos), Quaternion.identity);
    //     TextMeshPro textMeshPro = newText.GetComponentInChildren<TextMeshPro>();
    //     textMeshPro.text = text;
    // }

}
