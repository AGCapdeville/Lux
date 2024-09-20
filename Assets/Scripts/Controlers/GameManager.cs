using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using Scripts.Enums;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    private Board _board;
    private Player _player;
    private bool _player_clicked;
    private bool _grid_visible;

    public Transform heroTransform; // The hero's transform
    public int HeroMovement = 10;

    private static int _UnitIDCounter; // testing static access
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
        _UnitIDCounter = 0;

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


        // _board = new Board(rows, columns, spaceWidth, spaceHeight);
        // Game Board Setup END ------------------------------------ END

        _board = new Board();

        // Create Hero for Player & Add Hero to Board -------------- START
        Hero playerHero = new Hero(
            _UnitIDCounter++,
            HeroMovement,
            100,
            Vector3.zero,
            Direction.North,
            "Orion",
            "Triangle" // Prototype Prefab
        );
        _player = new Player(0, "P1");
        _player.AddHeroToParty(playerHero);
        _board.AddUnit(playerHero, playerHero.Position);
        // Create Hero for Player & Add Hero to Board -------------- END




        // CAMERA ------------------------------------------------- START
        // _gameCamera = Instantiate(Resources.Load<GameObject>("Prefabs/MainCamera"));
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
        Hero h = (Hero)_board.GetUnit(position);
        if (h != null && !_grid_visible && !_player_clicked)
        {
            _board.DisplayHeroGrid(h);
            _grid_visible = true;
        }
    }

    public void GameBoardHoverExit(Vector3 position)
    {
        Hero h = (Hero)_board.GetUnit(position);
        if (h != null && _grid_visible && !_player_clicked)
        {
            _board.HideMovementRange(h);
            _grid_visible = false;
        }
    }

    // Need to refine to movement tiles only later.. although it may already do that
    public void GameBoardClick(GameObject SpaceObject, SpaceType type)
    {
        Hero h = (Hero)_board.GetUnit(SpaceObject.transform.position);

        if (h != null) // clicked unit on board to select them
        {
            // store hero into gamemanager as selected hero
            _player.SelectedHero = h.HeroName;
            _player_clicked = !_player_clicked;
        }
        else if (_player_clicked) // resolve clicking on movment tile
        {
            _board.HideMovementRange(_player.Party[_player.SelectedHero]);
            if (type == SpaceType.Movement) 
            {   
                
                Queue<Space> route = _board.FindPath(
                    _player.Party[_player.SelectedHero].HeroGameObject.transform.position,
                     SpaceObject.transform.position
                );
                
                // Update MapData of Board to have Unit on respective space
                _board.UpdateUnit(_player.Party[_player.SelectedHero], SpaceObject.transform.position);
                
                _player.MoveTo(SpaceObject.transform.position, route, _player.Party[_player.SelectedHero]);
                
                _player.UpdateMovementRange(_board.GetMovementRange(_player.Party[_player.SelectedHero]), _player.Party[_player.SelectedHero]);
            }
            
            _player_clicked = false;
            _grid_visible = false;
        }
    }

}
