using System.Collections;
using System.Collections.Generic;
using Scripts.Enums;
using UnityEngine;

public class MouseObserver : MonoBehaviour
{
    private GameManager GM;

    void Start() 
    {
        GM = GameManager.Instance;
    }
    void OnEnable()
    {
        // Subscribe to all of the GameObject Spaces
        SpaceBehavior.OnSpaceHoverEnter += HandleSpaceHoverEnter;
        SpaceBehavior.OnSpaceHoverExit += HandleSpaceHoverExit;
        SpaceBehavior.OnSpaceClick += HandleSpaceClick;
    }

    void OnDisable()
    {
        // Un-Subscribe to all of the GameObject Spaces
        SpaceBehavior.OnSpaceHoverEnter -= HandleSpaceHoverEnter;
        SpaceBehavior.OnSpaceHoverExit -= HandleSpaceHoverExit;
        SpaceBehavior.OnSpaceClick -= HandleSpaceClick;

    }

    private void HandleSpaceHoverEnter(GameObject spaceObject)
    {
        // Need to reach out to the Game Manager, to figure out whats here:
        // (int, int) position = ((int)spaceObject.transform.position.x, (int)spaceObject.transform.position.z);
        GM.GameBoardHover(spaceObject.transform.position);
    }

    private void HandleSpaceHoverExit(GameObject spaceObject)
    {
        GM.GameBoardHoverExit(spaceObject.transform.position);
        // Additional logic for when hovering over a space object ends
    }

    private void HandleSpaceClick(GameObject spaceObject, SpaceType type)
    {
        GM.GameBoardClick(spaceObject, type);
        //Will process the gameboard click based on whihc space was clicked
    }

}
