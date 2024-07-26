using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveCube : MonoBehaviour
{
    public float speed = 1.0f;  // Speed of the cube's movement
    public Vector3 startPosition = new Vector3(10f, 0, -0.25f);

    public Vector3 fadeInPoint = new Vector3(9f, 0, -0.25f);
    public Vector3 endPosition = new Vector3(0f, 0, -0.25f);



    private Renderer objectRenderer;

    private Color red;
    private Color invisible;

    private float alpha = 0.0f;


    // Start is called before the first frame update
    void Start()
    {

        Debug.Log(transform.position);
        // transform.position = new Vector3(startPosition.x, 0f, startPosition.z);    

        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer == null)
        {
            Debug.LogError("No Renderer component found on this GameObject.");
            return;
        }


    }

    private float calcDistance(Vector3 start, Vector3 end) 
    {
        return Vector3.Distance(transform.position, end) / Vector3.Distance(start, end);
    }

    // Update is called once per frame
    void Update()
    {
        // float t = 0f;

        // if (transform.position.x <= fadeInPoint.x) {
        //     t = calcDistance(fadeInPoint, endPosition);
        // } else {
        //     t = calcDistance(startPosition, fadeInPoint);
        //     // objectRenderer.material.color = Color.Lerp(Color.clear, Color.red, t);
        // }

        // t = calcDistance(fadeInPoint, endPosition);


        // if (transform.position.x < endPosition.x) {
        //     transform.position = startPosition;    
        // } else {
        //     transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        // }

        transform.position -= new Vector3(speed * Time.deltaTime, 0f, 0f);


    }
}
