using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class rotate : MonoBehaviour
{
    public float rotationSpeed = 30f; // Rotation speed in degrees per second

    void Update()
    {
        // Rotate the text continuously
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
