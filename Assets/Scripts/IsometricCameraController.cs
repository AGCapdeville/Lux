using UnityEngine;
using Cinemachine;
using System;
using System.Collections.Generic;

public class IsometricCameraController : MonoBehaviour
{
    private CinemachineVirtualCamera _virtualCamera;
    private Transform target;
    public float zoomSpeed = 2f;
    public float minZoom = 10f;
    public float maxZoom = 25f;
    public float rotationDuration = 0.1f; // Duration for the rotation animation

    private CinemachineTransposer _transposer;
    private Vector3 _initialOffset;
    private Vector3 _targetOffset;
    private bool _isRotating;
    private float _rotationStartTime;

    public float cameraMoveSpeed = 20f; 
    private bool _shiftPressed = false;

    void Start()
    {
        // Get the CinemachineVirtualCamera component attached to this GameObject
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();

        // Ensure the virtual camera is not null
        if (_virtualCamera != null)
        {
            // Set the follow and look at targets to the same object this script is attached to
            target = _virtualCamera.Follow;

            // Get the transposer component
            _transposer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            _initialOffset = new Vector3(-10, 10, -10); // Set initial offset to maintain the 45-degree angle
            _transposer.m_FollowOffset = _initialOffset;
            _targetOffset = _initialOffset;
        }
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift)) {
            _shiftPressed = true;
        }

        if (_transposer != null)
        {
            // Zoom in and out with the mouse wheel
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            float newZoom = Mathf.Clamp(_transposer.m_FollowOffset.y - scroll * zoomSpeed, minZoom, maxZoom);
            
            // Maintain the original angle while zooming
            float zoomFactor = newZoom / _initialOffset.y;
            _transposer.m_FollowOffset = new Vector3(_initialOffset.x * zoomFactor, newZoom, _initialOffset.z * zoomFactor);

            // Rotate camera left when pressing the Q key and right when pressing the E key
            if (Input.GetKeyDown(KeyCode.Q)) {
                RotateCameraRight(newZoom);
            } else if (Input.GetKeyDown(KeyCode.E)) {
                RotateCameraLeft(newZoom);
            }

            if (_isRotating)
            {
                float t = (Time.time - _rotationStartTime) / rotationDuration;
                _transposer.m_FollowOffset = Vector3.Lerp(_transposer.m_FollowOffset, _targetOffset, t);
                if (t >= 1f)
                {
                    _isRotating = false;
                    _initialOffset = _targetOffset;
                }
            }
        }


        Vector3 inputDirection = new Vector3(moveX, 0, moveZ).normalized;
        // Transform input direction from local to world space relative to the camera's facing direction
        Vector3 transformedDirection = _virtualCamera.transform.TransformDirection(inputDirection);
        // Move the object
        Vector3 flatenDirection = new Vector3(transformedDirection.x, 0, transformedDirection.z);
        
        if (_shiftPressed) {
            _shiftPressed = false;
            _virtualCamera.Follow.transform.Translate(flatenDirection * cameraMoveSpeed * 2 * Time.deltaTime, UnityEngine.Space.World);
        } else {
            _virtualCamera.Follow.transform.Translate(flatenDirection * cameraMoveSpeed * Time.deltaTime, UnityEngine.Space.World);
        }

    }

    private void RotateCameraLeft(float zoomLevel)
    {
        _targetOffset = Quaternion.Euler(0, -90f, 0) * _initialOffset;
        float zoomFactor = zoomLevel / Mathf.Abs(_targetOffset.y);
        _targetOffset = new Vector3(_targetOffset.x * zoomFactor, zoomLevel, _targetOffset.z * zoomFactor);

        StartRotation();
    }

    private void RotateCameraRight(float zoomLevel)
    {
        _targetOffset = Quaternion.Euler(0, 90f, 0) * _initialOffset;
        float zoomFactor = zoomLevel / Mathf.Abs(_targetOffset.y);
        _targetOffset = new Vector3(_targetOffset.x * zoomFactor, zoomLevel, _targetOffset.z * zoomFactor);

        StartRotation();
    }

    private void StartRotation()
    {
        _isRotating = true;
        _rotationStartTime = Time.time;
    }

}