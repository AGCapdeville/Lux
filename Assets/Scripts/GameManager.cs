using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;



public class GameManager : MonoBehaviour
{

    private Player player;

    void Start()
    {
        // CreatePrefabsInCircle(40, spacePrefab);
        // GameBoard = CreateBoard(10, spacePrefab);
        // GameBoard GB = new GameBoard();
        // CreateJourneyTree();
        
        int numberOfRows = 5;
        int numberOfColumns = 5;
        int spaceWidth = 5;
        int spaceHeight = 5;

        List<GameObject> Board = GenerateBoard(numberOfRows, numberOfColumns, spaceWidth, spaceHeight);

        player = new Player(0, "Orion", 2, (0,0));
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))  // Change "Fire1" to the appropriate button name
        {
            // Button click detected
            
            player.movePlayer((5,5));
            
            // You can add your logic here, such as executing a function or changing game state
        }
    }



    List<GameObject> GenerateBoard(int numberOfRows, int numberOfColumns, int spaceWidth, int spaceHeight) {
        List<GameObject> board = new List<GameObject>();
        for (int row = 0; row < numberOfRows; row++)
        {
            for (int col = 0; col < numberOfColumns; col++)
            {
                GameObject space = new GameObject("space ["  + (row * spaceWidth) + "," + (col * spaceHeight) + "]");
                space.transform.localScale = new Vector3(spaceWidth, 1f, spaceHeight);
                space.transform.position = new Vector3(row * spaceWidth, 0f, col * spaceHeight);
                board.Add(space);
            }   
        }
        GenerateBoardGrid(numberOfRows, numberOfColumns, spaceWidth, spaceHeight);
        return board;
    }

    void GenerateBoardGrid(int numberOfRows, int numberOfColumns, int spaceWidth, int spaceHeight)
    {
        // Create a new mesh
        Mesh mesh = new Mesh();

        // Vertices array to hold positions of all vertices
        Vector3[] vertices = new Vector3[(numberOfRows + 1) * (numberOfColumns + 1)];

        // Generate vertices
        for (int col = 0; col <= numberOfColumns; col++)
        {
            for (int row = 0; row <= numberOfRows; row++)
            {
                vertices[col * (numberOfRows + 1) + row] = new Vector3((float)(row * spaceWidth - 0.5 * spaceWidth), 0, (float)(col * spaceHeight - 0.5 * spaceHeight));
            }
        }

        // Assign vertices to the mesh
        mesh.vertices = vertices;

        // Generate horizontal lines
        int numHorizontalLines = numberOfRows * (numberOfColumns + 1);
        int[] horizontalLineIndices = new int[numHorizontalLines * 2];
        int index = 0;
        for (int col = 0; col <= numberOfColumns; col++)
        {
            for (int row = 0; row < numberOfRows; row++)
            {
                horizontalLineIndices[index++] = col * (numberOfRows + 1) + row;
                horizontalLineIndices[index++] = col * (numberOfRows + 1) + row + 1;
            }
        }

        // Generate vertical lines
        int numVerticalLines = (numberOfRows + 1) * numberOfColumns;
        int[] verticalLineIndices = new int[numVerticalLines * 2];
        index = 0;
        for (int row = 0; row <= numberOfRows; row++)
        {
            for (int col = 0; col < numberOfColumns; col++)
            {
                verticalLineIndices[index++] = col * (numberOfRows + 1) + row;
                verticalLineIndices[index++] = (col + 1) * (numberOfRows + 1) + row;
            }
        }

        // Combine horizontal and vertical lines
        int[] allLineIndices = new int[numHorizontalLines * 2 + numVerticalLines * 2];
        horizontalLineIndices.CopyTo(allLineIndices, 0);
        verticalLineIndices.CopyTo(allLineIndices, numHorizontalLines * 2);

        // Assign the indices for the lines
        mesh.SetIndices(allLineIndices, MeshTopology.Lines, 0);

        // Create a mesh renderer and filter to render the mesh
        MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();
        MeshFilter filter = gameObject.AddComponent<MeshFilter>();

        // Assign the mesh to the filter
        filter.mesh = mesh;

        // Create a new material for the grid lines
        Material gridMaterial = new Material(Shader.Find("Unlit/Color"));
        Color gridColor = Color.white; // Color of the grid lines
        gridMaterial.color = gridColor;

        // Assign the material to the renderer
        renderer.material = gridMaterial;
    }

}
