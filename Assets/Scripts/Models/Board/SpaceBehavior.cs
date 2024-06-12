// using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpaceBehavior : MonoBehaviour
{
    private GameObject HighlightSpaceObject; // Reference to the spawned object A
    private AudioManager audioManager;

    // Define a delegate for hover events
    public delegate void HoverAction(GameObject spaceObject);
    public static event HoverAction OnSpaceHoverEnter;
    public static event HoverAction OnSpaceHoverExit;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    void OnMouseEnter()
    {
        if (HighlightSpaceObject == null)
        {
            OnSpaceHoverEnter?.Invoke(gameObject); // Trigger Hover Event
            
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
            // Trigger Hover Exit Event
            OnSpaceHoverExit?.Invoke(gameObject);
            // Destroy object A
            Destroy(HighlightSpaceObject);
            // Reset the reference
            HighlightSpaceObject = null;
        }
    }


}
