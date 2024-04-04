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

    private Player player;

    private Board board;
    private bool DISP = false;
    void Start()
    {
        int rows = 5;
        int columns = 5;
        int spaceWidth = 10; // Looks like 10 equates to 1 unit in plane
        int spaceHeight = 10;

        board = new Board(rows, columns, spaceWidth, spaceHeight);
        board.SetParent(gameObject); // Set the board game object's parent to GameManager Game Object

        player = new Player(0, "Orion", 2, Vector3.zero, board);
    }

    void Update()
    {
        // Testing
        // if (Input.GetButtonDown("Fire1"))  // Change "Fire1" to the appropriate button name
        // {
        //     player.MoveToRandomBoardSpace(board);   
        // }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!DISP) {
                player.DisplayMovementRange(board);
                DISP = true;
            } else {
                player.HideMovementRange(board);
                DISP = false;
            }
        }
    }

    public void ObjectInteract(string message, Vector3 targetPosition)
    {
        // Instruct the player to move to the clicked location
        // player.MoveTo(targetPosition);
        Debug.Log("Object Interaction:" + message);
        // TODO: Filter interations based on message or something.
        if (message == "movement_tile") {
            DISP = false;
            player.HideMovementRange(board);
            player.Move(new Vector3(targetPosition.x, 0f, targetPosition.z), board);
        }
    }
}
