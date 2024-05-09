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

    private Player Player;

    private Board Board;
    private bool DISP = false;
    void Start()
    {
        int rows = 5;
        int columns = 5;
        int spaceWidth = 10; // Looks like 10 equates to 1 unit in plane
        int spaceHeight = 10;

        Board = new Board(rows, columns, spaceWidth, spaceHeight);
        Board.SetParent(gameObject); // Set the board game object's parent to GameManager Game Object

        Player = new Player(0, "Orion", 2, Vector3.zero, Board);



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
            // if (!DISP) {
            //     Player.DisplayMovementRange(Board);
            //     DISP = true;
            // } else {
            //     Player.HideMovementRange(Board);
            //     DISP = false;
            // }

            Node n = new Node(0,0,1);
            n.Testing(Player.Piece.transform.position, Player.MovementGridSpaces, Board.SpaceWidth, Board.SpaceLength);
        }
        

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Player.rotateLeft();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Player.rotateRight();
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
            Player.HideMovementRange(Board);
            Player.Move(new Vector3(targetPosition.x, 0f, targetPosition.z), Board);
        }
    }
}
