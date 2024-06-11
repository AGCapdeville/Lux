using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MouseObserver : MonoBehaviour
{
    public Camera mainCamera; // Reference to the main camera (optional, can be assigned in Inspector)
    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition); // Create a ray from camera to mouse position
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) // Check for collision with any collider
        {
            GameObject objectHovered = hit.collider.gameObject;
            // Do something with the object the mouse is hovering over (e.g., print its name, change its color)
            if (objectHovered.tag == "Hero") {
                Debug.Log("Hovering over: " + objectHovered.name);
            }
        }
    }

}




