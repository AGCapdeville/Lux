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

    void Start()
    {
        int rows = 5;
        int columns = 5;
        int spaceWidth = 10; // Looks like 10 equates to 1 unit in plane
        int spaceHeight = 10;

        board = new Board(rows, columns, spaceWidth, spaceHeight);
        board.SetParent(gameObject); // Set the board game object's parent to GameManager Game Object

        player = new Player(0, "Orion", 2, (0,0), board);
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))  // Change "Fire1" to the appropriate button name
        {
            player.MoveToRandomBoardSpace(board);
            
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.DisplayMovementRange(board);
        }
    }
}
