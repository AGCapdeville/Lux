using System.Collections;
using System.Collections.Generic;
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
    }

    void OnDisable()
    {
        // Un-Subscribe to all of the GameObject Spaces
        SpaceBehavior.OnSpaceHoverEnter -= HandleSpaceHoverEnter;
        SpaceBehavior.OnSpaceHoverExit -= HandleSpaceHoverExit;
    }

    private void HandleSpaceHoverEnter(GameObject spaceObject)
    {
        // Need to reach out to the Game Manager, to figure out whats here:
        (int, int) position = ((int)spaceObject.transform.position.x, (int)spaceObject.transform.position.z);
        GM.GameBoardHover(position);
    }

    private void HandleSpaceHoverExit(GameObject spaceObject)
    {
        // Additional logic for when hovering over a space object ends
    }

}
