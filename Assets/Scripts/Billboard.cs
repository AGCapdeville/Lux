using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera _mainCamera;

    public Vector3 Forward; // The direction the character is facing
    public Vector3 normalizedCamera; // The normalized direction towards the camera

    // Gizmo color and length
    public Color gizmoColor = Color.red;
    public float gizmoLength = 2.0f;

    void Start() {
        // Assume the character is initially facing along the positive X axis
        Forward = new Vector3(1f, 0f, 0f);
        // _mainCamera = Camera.main;

        // if (_mainCamera == null)
        // {
        //     Debug.LogError("Main Camera not found!");
        // }

        _mainCamera = FindObjectOfType<Camera>();

        if (_mainCamera == null)
        {
            Debug.LogError("No camera found in the scene!");
        }
    }

    void Update()
    {
        if (_mainCamera != null)
        {
            // Calculate the direction vector from the character to the camera
            Vector3 direction = _mainCamera.transform.position - transform.position;

            // Flatten both vectors to the XZ plane
            Vector3 flattenedForward = new Vector3(Forward.x, 0f, Forward.z).normalized;
            Vector3 flattenedDirection = new Vector3(direction.x, 0f, direction.z).normalized;

            // Calculate the right vector based on the forward direction
            Vector3 rightVector = new Vector3(-flattenedForward.z, 0f, flattenedForward.x);

            // Calculate dot products to determine the relative position
            float dotRC = Vector3.Dot(rightVector, flattenedDirection);
            float dotDC = Vector3.Dot(flattenedForward, flattenedDirection);

            string result;

            if (dotDC > 0)
            {
                result = dotRC > 0 ? "Left Face" : "Right Face";
            }
            else
            {
                result = dotRC > 0 ? "Left Back" : "Right Back";
            }

            Debug.Log("Camera is on the: " + result);

            // Optionally: make the sprite face the camera (as you had before)
            transform.rotation = Quaternion.LookRotation(-flattenedDirection);
        }
    }

    // Draw a gizmo in the Scene view to show the forward direction
    void OnDrawGizmos()
    {
        // Set the gizmo color
        Gizmos.color = gizmoColor;

        // Calculate the end point of the forward vector
        Vector3 gizmoEndPoint = transform.position + (Forward.normalized * gizmoLength);

        // Draw a line representing the forward direction
        Gizmos.DrawLine(transform.position, gizmoEndPoint);

        // Optionally, draw an arrowhead at the end of the line
        Gizmos.DrawSphere(gizmoEndPoint, 0.1f); // Small sphere to indicate direction
    }
}
