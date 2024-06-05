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

    // void Awake()
    // {
    //     // GameObject player = new GameObject("Player_0"); // Init player 0
    //     GameObject heroPrefab = Resources.Load<GameObject>("Triangel");     
    //     Debug.Log(heroPrefab);
    //     Hero playerHero = new Hero(EntityIDCounter++, 5, 100, Vector3.zero, Direction.North, "Orion", heroPrefab);
    //     Player_0 = new Player(0, "P1", playerHero);

    //     PlayerList = new List<Player>();  // Initialize the list here
    //     PlayerList.Add(Player_0); // Add player 0 to list of players
    // }

    void Start()
    {
        GameObject space = Resources.Load<GameObject>("Triangle");
        GameObject obj = GameObject.Instantiate(space, Vector3.zero, Quaternion.identity);



        int rows = 5;
        int columns = 5;
        int spaceWidth = 10; // Looks like 10 equates to 1 unit in plane
        int spaceHeight = 10;

        // TODO: Update board to contain map_data? 
        Board = new Board(rows, columns, spaceWidth, spaceHeight);
        Board.SetParent(gameObject); // Set the board game object's parent to GameManager Game Object



        Board.SpawnPointer(1);

        // Instantiate player with chosen hero:
        // Board.AddEntity(Player.Hero);
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
