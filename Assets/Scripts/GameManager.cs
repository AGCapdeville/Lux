using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;



public class GameManager : MonoBehaviour
{
    // public GameObject gameManager;
    public static int EntityIDCounter = 0;
    private Board Board;
    private Player Player_0;
    private List<GameObject> Players;

    void Start()
    {

        GameObject player = new GameObject("Player_0");
        Players.Add(player);




        int rows = 5;
        int columns = 5;
        int spaceWidth = 10; // Looks like 10 equates to 1 unit in plane
        int spaceHeight = 10;

        // TODO: Update board to contain map_data? 
        Board = new Board(rows, columns, spaceWidth, spaceHeight);
        Board.SetParent(gameObject); // Set the board game object's parent to GameManager Game Object

        // Player chosen hero:
        // Load the hero prefab
        GameObject heroPrefab = Resources.Load<GameObject>("Triangel");
        Hero playerHero = new Hero(EntityIDCounter++, 5, 100, Vector3.zero, Direction.North, "Orion", heroPrefab);


        // Instantiate player with chosen hero:
        Player_0 = new Player(0, "P1", playerHero);
        Board.AddEntity(Player_0.Hero);
        // When the player wants to reference their Hero on the board, they will know their Position & ID
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     // TODO: Migrate over to board for player.
        //     // if (DISP) {
        //     //     DISP = false;
        //     //     Player.HideMovementRange(Board);
        //     // } else {
        //     //     DISP = true;
        //     //     Player.DisplayMovementRange(Board);
        //     // }
        // }

        // // if (Input.GetKeyDown(KeyCode.LeftArrow))
        // // {
        // //     Player.rotateLeft();
        // //     Console.Write("hello");
        // // }
 
        // // if (Input.GetKeyDown(KeyCode.RightArrow))
        // // {
        // //     Player.rotateRight();
        // // }


    }

    

    public void ObjectInteract(string message, Vector3 targetPosition)
    {
        // Instruct the player to move to the clicked location
        // player.MoveTo(targetPosition);
        Debug.Log("Object Interaction:" + message);

        // TODO: Filter interations based on message or something.
        // if (message == "movement_tile") {
        //     DISP = false;
        //     Player.HideMovementRange(Board);
        //     Player.Move(new Vector3(targetPosition.x, 0f, targetPosition.z), Board);
        // }
    }

}
