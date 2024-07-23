using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCube : MonoBehaviour
{
    public float speed = 1.0f;  // Speed of the cube's movement
    public float resetPosition = 5f;  // Position to reset to
    public float limitPosition = -5f;  // Position limit


    private Renderer objectRenderer;
    private Color originalColor;

    private float alpha = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(resetPosition, 0, -0.2f);    

        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer == null)
        {
            Debug.LogError("No Renderer component found on this GameObject.");
            return;
        }

        Color startColor = originalColor;
        startColor.a = 0.0f; // Start fully transparent
        objectRenderer.material.color = startColor;

    }

    // Update is called once per frame
    void Update()
    {
        Color startColor = originalColor; 
        if (transform.position.x < limitPosition) {
            transform.position = new Vector3(resetPosition, 0, -0.2f);

            alpha = 0.0f;
            startColor.a = alpha; // Start fully transparent
            objectRenderer.material.color = startColor;

        } else {

            alpha += 0.1f;
            startColor.a = alpha; // Start fully transparent
            objectRenderer.material.color = startColor;

            transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        }
    }
}
