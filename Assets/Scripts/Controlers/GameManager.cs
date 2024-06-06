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
    private Board Board;
    private Player Player;

    private static int EntityIDCounter; // testing static access
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }

    void Awake()
    {
        EntityIDCounter = 0;
        
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject); // Only allow one GameManager instance
        }
        else
        {
            _instance = this;
        }

        // GameObject space = Resources.Load<GameObject>("Space");
        // GameObject.Instantiate(space, Vector3.zero, Quaternion.identity);

        // Hero playerHero = new Hero(
        //     EntityIDCounter++,
        //     5,
        //     100,
        //     Vector3.zero,
        //     Direction.North,
        //     "Orion",
        //     "Triangel"
        // );

        // Player = new Player(0, "P1", playerHero);
    }


    void Start()
    {
        GameObject space = Resources.Load<GameObject>("Triangle");
        GameObject obj = GameObject.Instantiate(space, Vector3.zero, Quaternion.identity);

        int rows = 5;
        int columns = 5;
        int spaceWidth = 10; // Looks like 10 equates to 1 unit in plane
        int spaceHeight = 10;

        Board = new Board(rows, columns, spaceWidth, spaceHeight);
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
