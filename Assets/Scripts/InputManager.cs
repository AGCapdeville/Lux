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

    public static  GameManager _gmRewrite{ get; private set; }

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

        _gmRewrite = FindAnyObjectByType< GameManager>();
    }

    private void OnDestroy()
    {
        // Clean up and disable input actions when the object is destroyed
        InputActions.Disable();
    }

    public void GameBoardHover(Vector3 position) 
    {
        Hero h = (Hero)_gmRewrite._Board.GetUnit(position);
        if (h != null && !_gmRewrite.hero_grid_visible && !_gmRewrite.player_clicked)
        {
            _gmRewrite._Board.DisplayHeroGrid(h);
            _gmRewrite.hero_grid_visible = true;
        }
    }

    public void GameBoardHoverExit(Vector3 position)
    {
        Hero h = (Hero)_gmRewrite._Board.GetUnit(position);
        if (h != null && _gmRewrite.hero_grid_visible && !_gmRewrite.player_clicked)
        {
            _gmRewrite._Board.HideMovementRange(h);
            _gmRewrite.hero_grid_visible = false;
        }
    }

    public void GameBoardClick(GameObject SpaceObject, SpaceType type)
    {
        Hero h = (Hero)_gmRewrite._Board.GetUnit(SpaceObject.transform.position);

        if (h != null) // clicked unit on board to select them
        {
            // store hero into gamemanager as selected hero
            _gmRewrite._Player.SelectedHero = h.HeroName;
            _gmRewrite.player_clicked = !_gmRewrite.player_clicked;
        }
        else if (_gmRewrite.player_clicked) // resolve clicking on movment tile
        {
            _gmRewrite._Board.HideMovementRange(_gmRewrite._Player.Party[_gmRewrite._Player.SelectedHero]);
            if (type == SpaceType.Movement) 
            {   
                
                Queue<Space> route = _gmRewrite._Board.FindPath(
                    _gmRewrite._Player.Party[_gmRewrite._Player.SelectedHero].HeroGameObject.transform.position,
                     SpaceObject.transform.position
                );
                
                // Update MapData of Board to have Unit on respective space
                _gmRewrite._Board.UpdateUnit(_gmRewrite._Player.Party[_gmRewrite._Player.SelectedHero], SpaceObject.transform.position);
                
                _gmRewrite._Player.MoveTo(SpaceObject.transform.position, route, _gmRewrite._Player.Party[_gmRewrite._Player.SelectedHero]);
                
                _gmRewrite._Player.UpdateMovementRange(_gmRewrite._Board.GetMovementRange(_gmRewrite._Player.Party[_gmRewrite._Player.SelectedHero]), _gmRewrite._Player.Party[_gmRewrite._Player.SelectedHero]);
            }
            
            _gmRewrite.player_clicked = false;
            _gmRewrite.hero_grid_visible = false;
        }
    }
}
