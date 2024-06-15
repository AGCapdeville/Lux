using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{private Transform target; // The point the camera will focus on
    private Vector3 offset = new Vector3(0, 5, -10); // Offset from the target point
    private float smoothSpeed = 0.125f; // Speed of the smooth movement

    // Method to initialize the CameraController
    public void Initialize(Transform target, Vector3 offset, float smoothSpeed)
    {
        this.target = target;
        this.offset = offset;
        this.smoothSpeed = smoothSpeed;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // Desired position of the camera
            Vector3 desiredPosition = target.position + offset;

            // Smoothly interpolate between the current position and the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            // Make the camera look at the target point
            transform.LookAt(target);
        }
    }
}
