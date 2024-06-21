using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTile : MonoBehaviour
{
    private GameManager gameManager;

    private Camera camera;


    void Start()
    {
        // Find the GameManager in the scene
        gameManager = FindObjectOfType<GameManager>();
        camera = FindObjectOfType<Camera>();
        
    }
    void PlayerInteract()
    {
        // Inform the GameManager that this location was clicked
        gameManager.ObjectInteract("movement_tile", transform.position);

    }

    
    void OnMouseEnter()
    {
        // Check if object A has not been spawned yet
        // if (spawnedObject == null)
        // {
        //     // Spawn object A at the position of object B
        //     spawnedObject = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
        // }
    }


    void OnMouseExit()
    {
        // Check if object A exists before attempting to destroy it
        // if (spawnedObject != null)
        // {
        //     // Destroy object A
        //     Destroy(spawnedObject);
        //     // Reset the reference
        //     spawnedObject = null;
        // }
    }

    void OnMouseDown()
    {
        gameManager.GameBoardClick(gameObject.transform.position);
    }



    void Update()
    {   
        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the mouse cursor into the scene
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            // Check if the ray intersects with a collider
            if (Physics.Raycast(ray, out hit))
            {
                // gameManager.GameBoardClick(hit.collider.gameObject.transform.position);

                // Debug.Log(hit.collider.gameObject.name);
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
