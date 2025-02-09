// using System.Collections;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Scripts.Enums;

public class SpaceBehavior : MonoBehaviour
{
    private GameObject HighlightSpaceObject; // Reference to the spawned object A
    private GameManager gameManager;
    private AudioManager audioManager;

    // Define a delegate for hover events
    public delegate void HoverAction(GameObject spaceObject);
    public static event HoverAction OnSpaceHoverEnter;
    public static event HoverAction OnMovementTileHoverEnter;
    public static event HoverAction OnSpaceHoverExit;
    public static event HoverAction OnMovementTileHoverExit;
    
    public delegate void ClickAction(GameObject spaceObject, SpaceType type);
    public static event ClickAction OnSpaceClick;

    public SpaceType type = SpaceType.Default;
    private Renderer _renderer;
    private Material _hoverTile;
    private Material _movementTile;
    private Material _attackTile;


    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        //gameManager = FindObjectOfType<GameManager>();

        _renderer = GetComponent<Renderer>();
        _hoverTile = Resources.Load<Material>("Materials/YellowTile");
        _movementTile = Resources.Load<Material>("Materials/BlueTile");
        _attackTile = Resources.Load<Material>("Materials/RedTile");
    }

    public void SetType(SpaceType newType)
    {
        type = newType;
        switch (type)
        {
            case SpaceType.Movement:
                _renderer.material = _movementTile;
                break;
            case SpaceType.Default:
                _renderer.materials = new Material[0];
                break;
            case SpaceType.Attack:
                _renderer.material = _attackTile;
                break;
        }
    }

    void OnMouseEnter()
    {
        if (_renderer != null)
        {
            if (type != SpaceType.Movement)
            {
                _renderer.material = _hoverTile;
                OnSpaceHoverEnter?.Invoke(gameObject); // Trigger Hover Event
            } 
            else if (type == SpaceType.Movement) 
            {
                OnMovementTileHoverEnter?.Invoke(gameObject);
            } 
            
        }
        else
        {
            Debug.LogError("No Renderer component found on this GameObject.");
        }

        // if (!audioManager.sfxSource.isPlaying)
        audioManager.PlaySound("Point"); 
    }

    void OnMouseExit()
    {
        if (_renderer != null)
        {
            if (_renderer.material != null && type != SpaceType.Movement)
            {
                OnSpaceHoverExit?.Invoke(gameObject);
                _renderer.materials = new Material[0];
            }        
            else if (type == SpaceType.Movement) 
            {
                OnMovementTileHoverExit?.Invoke(gameObject);
            } 
        }
        else
        {
            Debug.LogError("No Renderer component found on this GameObject.");
        }
    }

    void OnMouseDown()
    {
        // Debug.Log("SpaceBehavior(L:81):" + type);
        OnSpaceClick?.Invoke(gameObject, type);
    }
}
