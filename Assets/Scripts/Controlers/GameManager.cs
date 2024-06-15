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
        
        // GAME MANAGER singelton logic ------------------------------------
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject); // Only allow one GameManager instance
        }
        else
        {
            _instance = this;
        }
        // GAME MANAGER singelton logic ------------------------------------
    }


    void Start()
    {
        // Game Board Setup START ------------------------------------ START
        int rows = 5;
        int columns = 5;
        int spaceWidth = 10; // Looks like 10 equates to 1 unit in plane
        int spaceHeight = 10;

        Board = new Board(rows, columns, spaceWidth, spaceHeight);
        // Game Board Setup END ------------------------------------ END

        // Create the Hero Unity Game Object
        Hero playerHero = new Hero(
            EntityIDCounter++,
            2,
            100,
            Vector3.zero,
            Direction.North,
            "Orion",
            "Triangle"
        );

        // Initialize Player Object, & attach Hero UnityGame Object
        Player = new Player(0, "P1", playerHero);

        // Link Players Hero to the Game Board  ------------------- START
        // Board.AddHero(playerHero);
        Board.AddEntity(playerHero);

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

    public void GameBoardHover(Vector3 position) {
        // Test to make sure that it was recieveing the Event
        Debug.Log("Entered:" + position.ToSafeString());

        Hero h = (Hero)Board.GetEntity(position, "hero");
        if (h != null) {
            // Hovering over hero:
            // Gen Range and Display Range
            Debug.Log(h.Yell());
            Board.DisplayHeroGrid(h);
        }

    }

    public void GameBoardHoverExit(Vector3 position) {
        // Test to make sure that it was recieveing the Event
        Debug.Log("Exited:" + position.ToSafeString());

        Hero h = (Hero)Board.GetEntity(position, "hero");
        if (h != null) {
            Board.HideHeroGrid(h);
        }

    }

}
