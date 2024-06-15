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
    private Board Board;
    private Player Player01;

    public Transform heroTransform; // The hero's transform
    private GameObject _gameCamera;

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

        // Create Hero for Player & Add Hero to Board -------------- START
        Hero playerHero = new Hero(
            EntityIDCounter++,
            2,
            100,
            Vector3.zero,
            Direction.North,
            "Orion",
            "Triangle"
        );
        Player01 = new Player(0, "P1", playerHero);
        Board.AddEntity(playerHero);
        // Create Hero for Player & Add Hero to Board -------------- END

        // CAMERA ------------------------------------------------- START
        _gameCamera = Instantiate(Resources.Load<GameObject>("MainCamera"));
        _gameCamera.transform.position = new Vector3(rows / 2 * spaceWidth, 20, -(columns / 2 * spaceWidth));
        CameraController cameraController = _gameCamera.GetComponent<CameraController>();
        cameraController.Initialize(heroTransform, new Vector3(rows / 2 * spaceWidth, 5, columns / 2 * spaceHeight), 0.125f);
        // Rename the instantiated object
        _gameCamera.name = "MainCamera";
        // CAMERA ------------------------------------------------- END
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
