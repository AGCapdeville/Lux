// using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceInteraction : MonoBehaviour
{
    private GameObject spawnedObject; // Reference to the spawned object A
    public GameObject objectToSpawn; // Reference to the spawned object A
    

    void OnMouseEnter()
    {
        // Check if object A has not been spawned yet
        if (spawnedObject == null)
        {
            // Spawn object A at the position of object B
            spawnedObject = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
        }
    }

    void OnMouseExit()
    {
        // Check if object A exists before attempting to destroy it
        if (spawnedObject != null)
        {
            // Destroy object A
            Destroy(spawnedObject);
            // Reset the reference
            spawnedObject = null;
        }
    }
}
