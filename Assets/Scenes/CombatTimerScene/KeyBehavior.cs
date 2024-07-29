using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KeyBehavior : MonoBehaviour
{
    public float speed = 0.25f;  // Speed of the cube's movement
    private Vector3 startPosition = new Vector3(10f, 0, -0.35f);
    private Vector3 endPosition = new Vector3(0f, 0, -0.35f);

    private Vector3 startFadeIn = new Vector3(10f, 0, -0.35f);
    private Vector3 endFadeIn = new Vector3(9f, 0, -0.35f);

    private Vector3 startFadeOut = new Vector3(1f, 0, -0.35f);
    private Vector3 endFadeOut = new Vector3(0f, 0, -0.35f);
    private Renderer objectRenderer;
    private float sinTime; 

    // Start is called before the first frame update
    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer == null)
        {
            Debug.LogError("No Renderer component found on this GameObject.");
            return;
        }
    }

    private float calcDistance(Vector3 start, Vector3 end) 
    {
        return Vector3.Distance(transform.localPosition, end) / Vector3.Distance(start, end);
    }

    private float evaluate(float x)
    {
        return 0.5f * Mathf.Sin(x - Mathf.PI / 2f) * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.localPosition.x > endFadeIn.x) {
            objectRenderer.material.color = Color.Lerp(Color.red, Color.clear, calcDistance(startFadeIn, endFadeIn));
        } else if (transform.localPosition.x < startFadeOut.x && transform.localPosition.x > 0f) {
            objectRenderer.material.color = Color.Lerp(Color.clear, Color.red, calcDistance(startFadeOut, endFadeOut));
        }

        if(transform.localPosition.x >= endPosition.x)
        {
            transform.localPosition -= new Vector3(Time.deltaTime * speed, 0, 0);
            // sinTime += Time.deltaTime * speed;
            // sinTime = Mathf.Clamp(sinTime, 0, Mathf.PI);
            // float t = evaluate(sinTime);
            // transform.localPosition = Vector3.Lerp(transform.localPosition, endPosition, t);
        }
        else 
        {
            transform.localPosition = startPosition;
            //sinTime = 0;
        }

    }
}
