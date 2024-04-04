using UnityEngine;

public class TriangleGenerator : MonoBehaviour
{
    public float height = 1f;
    public float width = 1f;
    public float length = 1f;

    void Start()
    {
        GenerateTriangle();
    }

    void GenerateTriangle()
    {
        // Define vertices
        Vector3 vertexA = new Vector3(0f, 0f, 0f);
        Vector3 vertexB = new Vector3(width, 0f, 0f);
        Vector3 vertexC = new Vector3(length / 2f, height, 0f);

        // Create GameObjects for vertices
        GameObject pointA = new GameObject("Point A");
        GameObject pointB = new GameObject("Point B");
        GameObject pointC = new GameObject("Point C");

        // Set positions for GameObjects
        pointA.transform.position = vertexA;
        pointB.transform.position = vertexB;
        pointC.transform.position = vertexC;

        // Create LineRenderers to draw edges
        DrawLine(pointA, pointB);
        DrawLine(pointB, pointC);
        DrawLine(pointC, pointA);
    }

    void DrawLine(GameObject pointA, GameObject pointB)
    {
        LineRenderer lineRenderer = pointA.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, pointA.transform.position);
        lineRenderer.SetPosition(1, pointB.transform.position);
    }
}
