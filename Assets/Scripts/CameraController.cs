using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{private Transform target; // The point the camera will focus on
    private Vector3 offset = new Vector3(0, 5, -10); // Offset from the target point
    private float smoothSpeed = 0.125f; // Speed of the smooth movement

    private GameObject follow;

    // Method to initialize the CameraController
    public void Spawn(Vector3 offset, float smoothSpeed)
    {
        // this.target = target;
        this.offset = offset;
        this.smoothSpeed = smoothSpeed;
        this.name = "MainCamera";

        follow = new GameObject("CameraFollow");
        follow.transform.position = offset;
    }

    void Start() {
        initCamera();
    }

    private void initCamera() {
        if (target != null)
        {
            spawnCamera();
        }
    }

    public void spawnCamera() {
        Vector3 desiredPosition = follow.transform.position + offset;

        // Smoothly interpolate between the current position and the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(follow.transform);
    }

}
