using Cinemachine;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Camera mainCamera;

    void Update()
    {
        if (mainCamera != null)
        {
            // Make the sprite face the camera
            Vector3 direction = mainCamera.transform.position - transform.position;
            direction.y = 0; // Keep the sprite upright, ignore y-axis rotation
            transform.rotation = Quaternion.LookRotation(-direction);
        }
    }
}
