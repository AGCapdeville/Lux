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
    private Board _Board;
    private Player _Player01;
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


        // _Board = new Board(rows, columns, spaceWidth, spaceHeight);
        // Game Board Setup END ------------------------------------ END

        // WIP : REDO BOARD

        _Board = new Board();

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
        _Player01 = new Player(0, "P1", playerHero);
        _Board.AddUnit(playerHero, playerHero.Position);
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
        Hero h = (Hero)_Board.GetUnit(position, "hero");
        if (h != null && !_grid_visible && !_player_clicked)
        {
            _Board.DisplayHeroGrid(h);
            _grid_visible = true;
        }
    }

    public void GameBoardHoverExit(Vector3 position)
    {
        Hero h = (Hero)_Board.GetUnit(position, "hero");
        if (h != null && _grid_visible && !_player_clicked)
        {
            _Board.HideMovementRange(h);
            _grid_visible = false;
        }
    }

    public void GameBoardClick(GameObject SpaceObject, SpaceType type)
    {
        Hero h = (Hero)_Board.GetUnit(SpaceObject.transform.position, "hero");
        if (h != null)
        {
            _player_clicked = !_player_clicked;
        }
        else if (_player_clicked)
        {
            _Board.HideMovementRange(_Player01.Hero);
            if (type == SpaceType.Movement) 
            {   
                
                Queue<Space> route = _Board.FindPath(_Player01.Hero.HeroGameObject.transform.position, SpaceObject.transform.position);
                Debug.Log(_Board.FindPath(_Player01.Hero.HeroGameObject.transform.position, SpaceObject.transform.position).ToString());
                
                foreach(var i in _Board.FindPath(_Player01.Hero.HeroGameObject.transform.position, SpaceObject.transform.position))
                {    
                    Debug.Log(i.SpaceGameObject.transform.position);
                }
                _Board.UpdateUnit((Unit)_Player01.Hero, SpaceObject.transform.position);
                _Player01.MoveTo(SpaceObject.transform.position, route);
                _Player01.UpdateMovementRange(_Board.GetMovementRange(_Player01.Hero));
            }
            
            _player_clicked = false;
            _grid_visible = false;
        }
    }

}
