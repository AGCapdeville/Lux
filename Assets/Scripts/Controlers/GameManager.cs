using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Scripts.Enums;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class  GameManager : MonoBehaviour
{
    public Board _Board { get; private set; }
    public Player _Player { get; private set; }
    private static int _UnitIDCounter; // testing static access
    public static  GameManager _instance { get; private set; }

    public GameState _current { get; private set; }
    private GameState _previous; 

    public  bool player_clicked;
    public  bool hero_grid_visible;

    [SerializeField] private string _MapPath; 

    delegate void sceneLoad();

    public void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }

        _UnitIDCounter = 0;
        player_clicked = false;
        hero_grid_visible = false;
        SceneManager.sceneLoaded += loadLevel;
        

    }

    public void loadLevel(Scene scene, LoadSceneMode mode)
    {   
        Debug.Log(scene);
        Debug.Log(mode);
        if (_current == GameState.CombatScreen)
        {
            //Do we want to desytory the board object when we are done with it?
            //Or do we want to just reinitalize all it's value and reconstruct the board?
            _Board = new Board();

            //Will there ever be more than one player? 
            _Player = new Player(0, "Player 1"); 

            //This will need to be removed eventually, somewhere else there should be a list of
            //all potental heros that is passed to the game manager to initialize
            Hero playerHero = new Hero(
                _UnitIDCounter++,
                10,
                100,
                Vector3.zero,
                Direction.North,
                "Orion",
                "Hero"
                );

            _Player.AddHeroToParty(playerHero);
            
            //This should proably use the player to add to the board, instead of using the hero object
            _Board.AddUnit(playerHero, playerHero.Position);


            Enemy enemy = new Enemy(
                _UnitIDCounter++,
                10,
                100,
                new Vector3(30,0,20),
                Direction.North,
                "Nightmare",
                "Nightmare"
            );

            Debug.Log(enemy.Position);

            _Board.AddUnit(enemy, enemy.Position);

        }
    }

    public void changeState(GameState gameState)
    {   
        _previous = _current;
        _current = gameState;
    }

}
