using UnityEngine;
using Cinemachine;

public class IsometricCameraController : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private Transform target;
    public float zoomSpeed = 2f;
    public float minZoom = 10f;
    public float maxZoom = 40f;
    public float rotationDuration = 0.1f; // Duration for the rotation animation

    private CinemachineTransposer transposer;
    private Vector3 initialOffset;
    private Vector3 targetOffset;
    private bool isRotating;
    private float rotationStartTime;

    void Start()
    {
        // Get the CinemachineVirtualCamera component attached to this GameObject
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        // Ensure the virtual camera is not null
        if (virtualCamera != null)
        {
            // Set the follow and look at targets to the same object this script is attached to
            target = virtualCamera.Follow;

            // Get the transposer component
            transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            initialOffset = new Vector3(-10, 10, -10); // Set initial offset to maintain the 45-degree angle
            transposer.m_FollowOffset = initialOffset;
            targetOffset = initialOffset;
        }
    }

    void Update()
    {
        if (transposer != null)
        {
            // Zoom in and out with the mouse wheel
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            float newZoom = Mathf.Clamp(transposer.m_FollowOffset.y - scroll * zoomSpeed, minZoom, maxZoom);
            
            // Maintain the original angle while zooming
            float zoomFactor = newZoom / initialOffset.y;
            transposer.m_FollowOffset = new Vector3(initialOffset.x * zoomFactor, newZoom, initialOffset.z * zoomFactor);

            // Rotate camera left when pressing the Q key and right when pressing the E key
            if (Input.GetKeyDown(KeyCode.Q)) {
                RotateCameraRight(newZoom);
            } else if (Input.GetKeyDown(KeyCode.E)) {
                RotateCameraLeft(newZoom);
            }

            if (isRotating)
            {
                float t = (Time.time - rotationStartTime) / rotationDuration;
                transposer.m_FollowOffset = Vector3.Lerp(transposer.m_FollowOffset, targetOffset, t);
                if (t >= 1f)
                {
                    isRotating = false;
                    initialOffset = targetOffset;
                }
            }
        }
    }

    private void RotateCameraLeft(float zoomLevel)
    {
        targetOffset = Quaternion.Euler(0, -90f, 0) * initialOffset;
        float zoomFactor = zoomLevel / Mathf.Abs(targetOffset.y);
        targetOffset = new Vector3(targetOffset.x * zoomFactor, zoomLevel, targetOffset.z * zoomFactor);

        StartRotation();
    }

    private void RotateCameraRight(float zoomLevel)
    {
        targetOffset = Quaternion.Euler(0, 90f, 0) * initialOffset;
        float zoomFactor = zoomLevel / Mathf.Abs(targetOffset.y);
        targetOffset = new Vector3(targetOffset.x * zoomFactor, zoomLevel, targetOffset.z * zoomFactor);

        StartRotation();
    }

    private void StartRotation()
    {
        isRotating = true;
        rotationStartTime = Time.time;
    }
}