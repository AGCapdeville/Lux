using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;



public class GameManager : MonoBehaviour
{
    void Start()
    {
        // CreatePrefabsInCircle(40, spacePrefab);
        // GameBoard = CreateBoard(10, spacePrefab);
        // GameBoard GB = new GameBoard();

        // CreateJourneyTree();
        
        CreateGameGrid(16,22,1,1);

    }

    void CreateGameGrid(int row, int column, int spaceWidth, int spaceHeight) {
        // Create a plane with a grid of 16 x 22
        // Each space on the plane is a 4 x 4 unit space (1 Unit is the default unit in unity)
        // Each space is blank but contains a space object.

        GameObject space = GameObject.CreatePrimitive(PrimitiveType.Plane);

        space.transform.localScale = new Vector3(spaceWidth, 1f, spaceHeight);
        Renderer renderer = space.GetComponent<Renderer>();
        Color newColor = HexToColor("006700"); // dark-green 006700
        renderer.material.color = newColor;

        for (int r = 0; r < row; r++)
        {
            for (int c = 0; c < column; c++)
            {
                space.transform.position = new Vector3(r, 0f, c);
                GameObject.Instantiate(space);
            }   
        }

        // GameObject space = new GameObject("space");
        // space.transform.localScale = new Vector3(spaceWidth, 1f, spaceHeight); // Adjust scale as needed


        // space.transform.position = new Vector3(0f, 0f, 0f);


        // GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        // plane.transform.localScale = new Vector3(10f, 1f, 10f); // Adjust scale as needed
        // Optionally, you can adjust the position, rotation, and scale of the plane
        // plane.transform.position = new Vector3(0f, 0f, 0f);
        // Renderer renderer = plane.GetComponent<Renderer>();

        // Check if the Renderer component exists
        // if (renderer != null)
        // {
        //     Color newColor = HexToColor("006700"); // dark-green 006700
        //     // Set the color of the plane
        //     renderer.material.color = newColor;
        // }
        // else
        // {
        //     Debug.LogWarning("Renderer component not found!");
        // }


    }

    // Function to convert a hexadecimal color code to a Color struct
    Color HexToColor(string hex)
    {
        // Remove '#' from the beginning if present
        hex = hex.TrimStart('#');

        // Convert the hexadecimal color code to a Color struct
        Color color = new Color();
        UnityEngine.ColorUtility.TryParseHtmlString("#" + hex, out color);

        return color;
    }
}


// WIP
//     void CreateJourneyTree() {
//         // Create starting node
//         GameObject SpacePrefab = Resources.Load<GameObject>("SpacePrefab");
//         GameObject startingPath = Instantiate(SpacePrefab, Vector3.zero, Quaternion.identity);
//         int firstRowOfSpaces = UnityEngine.Random.Range(2,5);
//         for (int i = 0; i < firstRowOfSpaces; i++)
//         {
//             GameObject path = Instantiate(SpacePrefab, new Vector3(7f,0,7f), Quaternion.identity);
//             float d = Vector3.Distance(startingPath.transform.position, path.transform.position);
//             // creae the line that will go between the two paths.
//             GameObject line = GameObject.CreatePrimitive(PrimitiveType.Cube);
//             // Z: is the forward direction (important for turning direction)
//             line.transform.localScale = new Vector3(0.25f, 0.25f, d);
//             // set position of line to be in the middle of the two paths.
//             // rotate the line so that the line connects the two paths.
//             line.transform.position = CalculateMidpoint(startingPath.transform.position, nextPathZero.transform.position);
//             // Calculate the direction vector from the starting point to the next point
//             Vector3 direction = (nextPathZero.transform.position - startingPath.transform.position).normalized;
//             // Calculate the rotation to align the line with the direction vector
//             Quaternion rotation = Quaternion.LookRotation(direction);
//             // Set the rotation of the line
//             line.transform.rotation = rotation;
//         }
//         // Create and link all 12 levls
//         // int numberOfLevelsOnFloor = 12;
//         // for (int level = 1; level < numberOfLevelsOnFloor; level++)
//         // {
//         //     List<Location> FirstRow = new List<Location>();;
//         //     int numberOfCurrentLocations = UnityEngine.Random.Range(2, 5);
//         //     for (int n = 0; n < numberOfCurrentLocations; n++)
//         //     {
//         //         Location location = new Location(1, "Location Name");
//         //         FirstRow.Add(location);
//         //     }
//         //     level++;
//         //     List<Location> SecondRow = new List<Location>();;
//         //     numberOfCurrentLocations = UnityEngine.Random.Range(2, 5);
//         //     for (int n = 0; n < numberOfCurrentLocations; n++)
//         //     {
//         //         Location location = new Location(1, "Location Name");
//         //         SecondRow.Add(location);
//         //     }
//         //     int availablePathCount = SecondRow.Count;
//         //     int pathsToMake = UnityEngine.Random.Range(1, availablePathCount);
//         //     for (int s = 0; s < SecondRow.Count; s++)
//         //     {
//         //     }
//         //     // Loop through each location in first row and connect paths to next row
//         //     for (int firstLocation = 1; firstLocation < FirstRow.Count; firstLocation++)
//         //     {
//         //         availablePathCount = 0;
//         //         // Count number of available locations to path to in second row.
//         //         foreach (var secondLocation in SecondRow)
//         //         {
//         //         }
//         //     }
//         //     // 1)   Create nodes left to right 
//         //     // 1.5) FirstRow = firstRow[Nodes]
//         //     // 2)   Create next row from left to right
//         //     // 2.5) SecondRow = secondRow[Nodes]
//         //     // 3)   Loop through each node in FirstRow[nodes]
//         //     // 3.1)   for each node from left to right, calculate how any nodes from secondRow the node will go to
//         //     // 3.2)   repeat until each node in the first lane is pointing to 1 node in the next row.
//         //     // 3.3)   if all nodes are pointed to at a node, then just point this node to the furthest right node.
//         //     // 4)     spawn nodes
//         // }

//     }
//     Vector3 CalculateMidpoint(Vector3 point1, Vector3 point2)
//     {
//         // Calculate the midpoint using vector arithmetic
//         return ((point1 + point2) / 2f);
//     }
// }



// TESTING OUT MAKING LINES Between spaces
//         GameObject nextPathOne = Instantiate(SpacePrefab, new Vector3(7f,0,7f), Quaternion.identity);

//         float distance0 = Vector3.Distance(startingPath.transform.position, nextPathZero.transform.position);
//         float distance1 = Vector3.Distance(startingPath.transform.position, nextPathOne.transform.position);

//         // creae the line that will go between the two paths.
//         GameObject line = GameObject.CreatePrimitive(PrimitiveType.Cube);
//         // Z: is the forward direction (important for turning direction)
//         line.transform.localScale = new Vector3(0.25f, 0.25f, distance0);
//         // set position of line to be in the middle of the two paths.
//         // rotate the line so that the line connects the two paths.

//         line.transform.position = CalculateMidpoint(startingPath.transform.position, nextPathZero.transform.position);
//         // Calculate the direction vector from the starting point to the next point
//         Vector3 direction = (nextPathZero.transform.position - startingPath.transform.position).normalized;
//         // Calculate the rotation to align the line with the direction vector
//         Quaternion rotation = Quaternion.LookRotation(direction);
//         // Set the rotation of the line
//         line.transform.rotation = rotation;



//         // creae the line that will go between the two paths.
//         GameObject line2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
//         line2.transform.localScale = new Vector3(0.25f, 0.25f, distance1);
//         // rotate the line so that the line connects the two paths.
//         // create the function that goes here to rotate the line two angle between the two points.

//         line2.transform.position = CalculateMidpoint(startingPath.transform.position, nextPathOne.transform.position);
//         // Calculate the direction vector from the starting point to the next point
//         direction = (nextPathOne.transform.position - startingPath.transform.position).normalized;
//         // Calculate the rotation to align the line with the direction vector
//         rotation = Quaternion.LookRotation(direction);
//         // Set the rotation of the line
//         line2.transform.rotation = rotation;