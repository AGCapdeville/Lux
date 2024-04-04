using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTile : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        // Find the GameManager in the scene
        gameManager = FindObjectOfType<GameManager>();
    }
    void PlayerInteract()
    {
        // Inform the GameManager that this location was clicked
        gameManager.ObjectInteract("movement_tile", transform.position);
    }

    void Update()
    {
        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the mouse cursor into the scene
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray intersects with a collider
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the collider belongs to the object we want to detect clicks on
                if (hit.collider.gameObject == gameObject)
                {
                    // Object clicked, do something...
                    PlayerInteract();
                }
            }
        }
    }
}
