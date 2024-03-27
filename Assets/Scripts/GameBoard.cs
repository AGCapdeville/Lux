using System;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameBoard
{
    // Properties

    // Constructor
    public GameBoard()
    {
        List<BoardSpace> GameBoard = CreateBoard(20);
    }

    List<BoardSpace> CreateBoard(int numberOfSpaces)
    {
        GameObject SpacePrefab = Resources.Load<GameObject>("SpacePrefab");
        Renderer renderer = SpacePrefab.GetComponent<Renderer>();

        List<BoardSpace> Board = new List<BoardSpace>();
        return Board;
        // calculate the angle between spaces on the board
        float angleBetweenSpaces = 360 / numberOfSpaces;

        // calculate the length between each space
        float equalSpaceAngles = (180 - angleBetweenSpaces) / 2;
        int spacer = 1;
        float lengthBetweenSpaces = (renderer.bounds.size.x * 2) + spacer;

        double top = Math.Sin(equalSpaceAngles * Math.PI / 180);
        double bot = Math.Sin(angleBetweenSpaces * Math.PI / 180);

        // calculate the distance away from the orgin each space will be placed
        double boardRadius =  lengthBetweenSpaces * (top / bot);

        // Loop through each degree spacing to spawn prefabs in a circle
        for (int i = 0; i < numberOfSpaces; i++)
        {
            // Calculate the angle for the i(th) placed space
            float angle = i * Mathf.Deg2Rad * angleBetweenSpaces;

            // Calculate the spawn position on the circle
            float x = (float)(Mathf.Cos(angle) * boardRadius);
            float z = (float)(Mathf.Sin(angle) * boardRadius);
            Vector3 spawnPosition = new Vector3(x, 0f, z);

            // Spawn the prefab at the calculated position
            // Instantiate(SpacePrefab, spawnPosition, Quaternion.identity);
            GameObject.Instantiate(SpacePrefab, spawnPosition, Quaternion.identity);

            spawnText(i.ToString(), x, 1, z);
            Board.Add(new BoardSpace(i)); 
        }
    }


    // Need to move this to a debug class or something:
    void spawnText(string text, float xpos, float ypos, float zpos) {
        GameObject loadedTextPrefab = Resources.Load<GameObject>("DebugTextPrefab");
        GameObject newText = GameObject.Instantiate(loadedTextPrefab, new Vector3(xpos, ypos, zpos), Quaternion.identity);
        TextMeshPro textMeshPro = newText.GetComponentInChildren<TextMeshPro>();
        textMeshPro.text = text;
    }
}
