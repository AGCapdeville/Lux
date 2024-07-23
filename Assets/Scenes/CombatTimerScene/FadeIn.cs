using UnityEngine;

public class FadeIn : MonoBehaviour
{
    public float duration = 5.0f; // Duration of the fade-in in seconds
    private Renderer objectRenderer;
    private Color originalColor;
    private float startTime;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer == null)
        {
            Debug.LogError("No Renderer component found on this GameObject.");
            return;
        }

        originalColor = objectRenderer.material.color;
        Color startColor = originalColor;
        startColor.a = 0.0f; // Start fully transparent
        objectRenderer.material.color = startColor;
        startTime = Time.time;
    }

    void Update()
    {
        float elapsed = Time.time - startTime;
        float alpha = Mathf.Clamp01(elapsed / duration);

        Color newColor = originalColor;
        newColor.a = alpha;
        objectRenderer.material.color = newColor;

        if (alpha == 1)
        {
            // Optionally, disable the script to stop further updates
            this.enabled = false;
        }
    }
}
