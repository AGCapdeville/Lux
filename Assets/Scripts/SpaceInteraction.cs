using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceInteraction : MonoBehaviour
{
    private GameObject spawnedObject; // Reference to the spawned object A
    public GameObject objectToSpawn; // Reference to the spawned object A
    
    private LineRenderer lineRenderer;
    private GameObject lineObj;

    // Then fetch map data:
    // private var blockedLocations = new List<(int, int)> {(0,3),(1,2)};
    // private int distance = 10;
    // private var mapData = GenerateMap(blockedLocations, distance);

    // Then fetch character position [x,z]
    // Node playerNode = new Node(0,0);
    // playerNode.Type = SpaceEnum.Player;

    // Then we need to get the cube we clicked on position [x,z]
    // Node endNode = new Node(0,40);

    // Calculate shortest path to clicked location
    // Pathing playerPathing = new Pathing(mapData, distance);
    // playerPathing.ClosedList[(playerNode.Position["x"], playerNode.Position["y"])] = playerNode;
    // List<Node> path = playerPathing.FindPath(playerNode, endNode);

    private List<(int, int)> Blocked_Locations;
    private int Distance = 10;
    private Dictionary<(int, int), Node> Map_Data;

    void Start()
    {
        Blocked_Locations = new List<(int, int)> {(0,3),(1,2)};
        Map_Data = GenerateMap(Blocked_Locations, Distance);
    }


    public static Dictionary<(int, int), Node> GenerateMap(List<(int, int)> blocked, int distance)
    {
        var genMap = new Dictionary<(int, int), Node>();
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                var node = new Node(x * distance, y * distance);
                if (blocked.Contains((x, y)))
                {
                    node.Type = SpaceEnum.Block;
                }
                genMap[(x * distance, y * distance)] = node;
            }
        }
        return genMap;
    }

    void DrawPath(List<Node> path)
    {
        if (lineRenderer == null)
        {
            lineObj = new GameObject("PathLine");
            lineRenderer = lineObj.AddComponent<LineRenderer>();

            // Configure the LineRenderer
            lineRenderer.startWidth = 0.2f;
            lineRenderer.endWidth = 0.2f;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Use a default material
            lineRenderer.positionCount = path.Count;
            lineRenderer.useWorldSpace = true;
        }

        for (int i = 0; i < path.Count; i++)
        {
            Vector3 position = new Vector3(path[i].Position["x"], 0, path[i].Position["y"]);
            lineRenderer.SetPosition(i, position);
        }
    }

    void RemovePath()
    {
        if (lineObj != null)
        {
            Destroy(lineObj);
            lineObj = null;
            lineRenderer = null;
        }
    }

    void OnMouseEnter()
    {
        // Check if object A has not been spawned yet
        if (spawnedObject == null)
        {
            // Spawn object A at the position of object B
            spawnedObject = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
        }

        // Then fetch character position [x,z]
        Node playerNode = new Node(0, 0);
        playerNode.Type = SpaceEnum.Player;

        // Then we need to get the cube we clicked on position [x,z]
        Node endNode = new Node((int)transform.position.x, (int)transform.position.z);

        // Calculate shortest path to clicked location
        Pathing playerPathing = new Pathing(Map_Data, Distance);
        playerPathing.ClosedList[(playerNode.Position["x"], playerNode.Position["y"])] = playerNode;

        // !!PATHING!!
        List<Node> path = playerPathing.FindPath(playerNode, endNode);

        DrawPath(path);
    }

    void OnMouseExit()
    {
        // Check if object A exists before attempting to destroy it
        if (spawnedObject != null)
        {
            // Destroy object A
            Destroy(spawnedObject);
            // Reset the reference
            spawnedObject = null;
        }

        // Remove the drawn path
        RemovePath();
    }
}
