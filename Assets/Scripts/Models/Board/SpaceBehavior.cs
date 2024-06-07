// using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBehavior : MonoBehaviour
{
    private GameObject HighlightSpaceObject; // Reference to the spawned object A

    private AudioManager audioManager;
    
    private void Start()
    {
        // ISSAC TODO: Turn this into a event to signal a sound to be played,
        // Not sure how it will work of course, this could be left alone. Just not sure
        // If it would be better if it was decoupled.
        audioManager = FindObjectOfType<AudioManager>();
    }

    void OnMouseEnter()
    {
        // if there is a hero standing on the space:
        //    display movement gird around hero.
        
        // else just display 
        // Check if object A has not been spawned yet
        if (HighlightSpaceObject == null)
        {
            HighlightSpaceObject = Instantiate(
                Resources.Load<GameObject>("HighlightTerrain"),
                transform.position,
                Quaternion.identity
            );
            
            if (!audioManager.sfxSource.isPlaying)
                audioManager.PlaySound("Bing");
        }
    }

    void OnMouseExit()
    {
        // Check if object A exists before attempting to destroy it
        if (HighlightSpaceObject != null)
        {
            // Destroy object A
            Destroy(HighlightSpaceObject);
            // Reset the reference
            HighlightSpaceObject = null;
        }
    }
}
