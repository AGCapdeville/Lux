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


// TODO: could run into issues when switching context of inputs/actions players will be using in different scenes.
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public static  GameManager _GameManager{ get; private set; }

    public PlayerInputActions InputActions { get; private set; }

    private void Awake()
    {
        // Ensure there's only one instance of InputManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }

        // Initialize the PlayerInputActions
        InputActions = new PlayerInputActions();
        InputActions.Enable(); // Enable input actions

        _GameManager = FindAnyObjectByType< GameManager>();
    }

    private void OnDestroy()
    {
        // Clean up and disable input actions when the object is destroyed
        InputActions.Disable();
    }

    public void GameBoardHover(Vector3 position) 
    {
        Unit unit = _GameManager._Board.GetUnit(position);
        if (unit != null && unit.Type == "hero") {
            Hero h = (Hero)unit;
            if (h != null && !_GameManager.hero_grid_visible && !_GameManager.player_clicked)
            {
                _GameManager._Board.DisplayHeroGrid(h);
                _GameManager.hero_grid_visible = true;
            }
        }
    }

    public void GameBoardHoverExit(Vector3 position)
    {
        Unit unit = _GameManager._Board.GetUnit(position);
        if (unit != null && unit.Type == "hero") {
            Hero h = (Hero)_GameManager._Board.GetUnit(position);
            if (h != null && _GameManager.hero_grid_visible && !_GameManager.player_clicked)
            {
                _GameManager._Board.HideMovementRange(h);
                _GameManager.hero_grid_visible = false;
            }
        }
    }

    public void GameBoardMovementTileHoverEnter(Vector3 position) 
    {
        Vector3 SelectedHeroPosition = _GameManager._Player.Party[_GameManager._Player.SelectedHero].Position;
        Queue<Space> path = _GameManager._Board.FindPath(SelectedHeroPosition, position);
        _GameManager._Board.DrawPath(path);
    }

    public void GameBoardMovementTileHoverExit(Vector3 position)
    {
        _GameManager._Board.DeletePath();
    }

    public void GameBoardClick(GameObject SpaceObject, SpaceType type)
    {
        Hero h = (Hero)_GameManager._Board.GetUnit(SpaceObject.transform.position);

        if (h != null && _GameManager._Player.SelectedHero == "") // clicked unit on board to select them
        {
            // store hero into gamemanager as selected hero
            _GameManager._Player.SelectedHero = h.HeroName;
            _GameManager.player_clicked = !_GameManager.player_clicked;

        }
        else if (_GameManager.player_clicked) // resolve clicking on movment tile
        {
            _GameManager._Board.HideMovementRange(_GameManager._Player.Party[_GameManager._Player.SelectedHero]);

            if (type == SpaceType.Movement && h == null)
            {   
                
                Queue<Space> route = _GameManager._Board.FindPath(
                    _GameManager._Player.Party[_GameManager._Player.SelectedHero].HeroGameObject.transform.position,
                     SpaceObject.transform.position
                );
                
                // Update MapData of Board to have Unit on respective space
                _GameManager._Board.UpdateUnit(_GameManager._Player.Party[_GameManager._Player.SelectedHero], SpaceObject.transform.position);
                
                _GameManager._Player.MoveTo(SpaceObject.transform.position, route, _GameManager._Player.Party[_GameManager._Player.SelectedHero]);
        
                _GameManager._Player.UpdateMovementRange(_GameManager._Board.GetMovementRange(_GameManager._Player.Party[_GameManager._Player.SelectedHero]), _GameManager._Player.Party[_GameManager._Player.SelectedHero]);
        
            }

            if (type == SpaceType.Default && h != null )
            {
                h.AttackRange = _GameManager._Board.GetAttackRange(h);
                _GameManager._Board.DisplayAttackRange(h);
            }

            _GameManager._Player.SelectedHero = "";            
            _GameManager.player_clicked = false;
            _GameManager.hero_grid_visible = false;
        }
    }


}
