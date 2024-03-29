
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player
{
    public int playerID { get; }
    public string heroName { get; }
    public int movement { get; }
    public GameObject playerPiece {get; set;}


    // Constructor to initialize event_id and event_name
    public Player(int id, string name, int initMovement, (int, int) initPosition)
    {
        playerID = id;
        heroName = name;
        movement = initMovement;
        playerPiece = new GameObject("player");
        playerPiece.transform.position = new Vector3(initPosition.Item1, 0f, initPosition.Item2);

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // Set the cube's position, rotation, and scale (optional)
        cube.transform.localScale = new Vector3(1f, 1f, 1f); // Example scale
        // Set the cube's parent to parent game object
        cube.transform.parent = playerPiece.transform;

    }

    public void movePlayer((int, int) newPosition) {
        playerPiece.transform.position = new Vector3(newPosition.Item1, 0f, newPosition.Item2);
    }
}