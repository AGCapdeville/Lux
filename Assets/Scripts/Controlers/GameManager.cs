using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Scripts.Enums;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    private Board _Board;
    private Player _Player01;
    private bool _player_clicked;
    private bool _grid_visible;

    public Transform heroTransform; // The hero's transform
    public int HeroMovement = 2;

    private static int _EntityIDCounter; // testing static access
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
        _EntityIDCounter = 0;

        _player_clicked = false;
        _grid_visible = false;

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
        // Lock the cursor and make it invisible
        // UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        // UnityEngine.Cursor.visible = false;

        // Game Board Setup START ------------------------------------ START
        int rows = 5;
        int columns = 5;
        int spaceWidth = 10; // Looks like 10 equates to 1 unit in plane
        int spaceHeight = 10;

        _Board = new Board(rows, columns, spaceWidth, spaceHeight);
        // Game Board Setup END ------------------------------------ END

        // Create Hero for Player & Add Hero to Board -------------- START
        Hero playerHero = new Hero(
            _EntityIDCounter++,
            HeroMovement,
            100,
            Vector3.zero,
            Direction.North,
            "Orion",
            "Triangle"
        );
        _Player01 = new Player(0, "P1", playerHero);
        _Board.AddEntity(playerHero);
        // Create Hero for Player & Add Hero to Board -------------- END

        // CAMERA ------------------------------------------------- START
        // _gameCamera = Instantiate(Resources.Load<GameObject>("MainCamera"));
        // _gameCamera.transform.position = new Vector3(rows / 2 * spaceWidth, 20, -(columns / 2 * spaceWidth));
        // CameraController cameraController = _gameCamera.GetComponent<CameraController>();
        // cameraController.Spawn(new Vector3(rows / 2 * spaceWidth, 5, columns / 2 * spaceHeight), 0.125f);
        // CAMERA ------------------------------------------------- END
    }

    public void ObjectInteract(string message, Vector3 targetPosition)
    {
        // Instruct the player to move to the clicked location
        // player.MoveTo(targetPosition);
        // Debug.Log("Object Interaction:" + message);

        // TODO: Filter interations based on message or something.
        // if (message == "movement_tile") {
        //     DISP = false;
        //     Player.HideMovementRange(Board);
        //     Player.Move(new Vector3(targetPosition.x, 0f, targetPosition.z), Board);
        // }
    }

    public void GameBoardHover(Vector3 position)
    {
        Hero h = (Hero)_Board.GetEntity(position, "hero");
        if (h != null && !_grid_visible && !_player_clicked)
        {
            _Board.DisplayHeroGrid(h);
            _grid_visible = true;
        }
    }

    public void GameBoardHoverExit(Vector3 position)
    {
        Hero h = (Hero)_Board.GetEntity(position, "hero");
        if (h != null && _grid_visible && !_player_clicked)
        {
            _Board.HideMovementRange(h);
            _grid_visible = false;
        }
    }

    public void GameBoardClick(GameObject SpaceObject, SpaceType type)
    {
        Hero h = (Hero)_Board.GetEntity(SpaceObject.transform.position, "hero");
        if (h != null)
        {
            _player_clicked = !_player_clicked;
        }
        else if (_player_clicked)
        {
            _Board.HideMovementRange(_Player01.Hero);
            if (type == SpaceType.Movement) 
            {
                _Board.UpdateEnity((Entity)_Player01.Hero, SpaceObject.transform.position);
                _Player01.MoveTo(SpaceObject.transform.position);
                _Player01.UpdateMovementRange(_Board.GetMovementRange(_Player01.Hero));
            }
            
            _player_clicked = false;
            _grid_visible = false;
        }
    }

}
