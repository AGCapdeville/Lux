using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject _follow;
    private Transform _target; // The point the camera will focus on
    private Vector3 _offset = new Vector3(0, 5, -10); // Offset from the target point
    private float _smoothSpeed = 0.125f; // Speed of the smooth movement

    // Method to initialize the CameraController
    public void Spawn(Vector3 offset, float smoothSpeed)
    {
        // this.target = target;
        _offset = offset;
        _smoothSpeed = smoothSpeed;
        name = "MainCamera";

        _follow = new GameObject("CameraFollow");
        _follow.transform.position = offset;
    }

    void Start()
    {
        initCamera();
    }

    private void initCamera()
    {
        if (_target != null)
        {
            spawnCamera();
        }
    }

    public void spawnCamera()
    {
        Vector3 desiredPosition = _follow.transform.position + _offset;

        // Smoothly interpolate between the current position and the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(_follow.transform);
    }

}
