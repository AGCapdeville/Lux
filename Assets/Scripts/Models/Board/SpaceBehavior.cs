// using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SpaceBehavior : MonoBehaviour
{
    private GameObject HighlightSpaceObject; // Reference to the spawned object A
    private GameManager gameManager;
    private AudioManager audioManager;

    // Define a delegate for hover events
    public delegate void HoverAction(GameObject spaceObject);
    public static event HoverAction OnSpaceHoverEnter;
    public static event HoverAction OnSpaceHoverExit;

    public delegate void ClickAction(GameObject spaceObject);

    public static event ClickAction OnSpaceClick;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        gameManager = FindObjectOfType<GameManager>();
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

    void OnMouseDown()
    {
        OnSpaceClick?.Invoke(gameObject);
        //Will Invoke based on which space was clicked
    }
}
