using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;



public class GameManager : MonoBehaviour
{
    // public GameObject gameManager;

    private Player Player;

    private Board Board;
    private bool DISP = false;


    private LineRenderer lineRenderer;
    void DrawPath(List<Node> path)
    {
        if (lineRenderer == null)
        {
            GameObject lineObj = new GameObject("PathLine");
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



    void Start()
    {
        int rows = 5;
        int columns = 5;
        int spaceWidth = 10; // Looks like 10 equates to 1 unit in plane
        int spaceHeight = 10;

        // TODO: Update board to contain map_data? 
        Board = new Board(rows, columns, spaceWidth, spaceHeight);
        Board.SetParent(gameObject); // Set the board game object's parent to GameManager Game Object

        Player = new Player(0, "Orion", 2, Vector3.zero, Board);
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (DISP) {
                DISP = false;
                Player.HideMovementRange(Board);
            } else {
                DISP = true;
                Player.DisplayMovementRange(Board);
            }
        }
        

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Player.rotateLeft();
            Console.Write("hello");
        }
 
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Player.rotateRight();
        }


    }

    public void ObjectInteract(string message, Vector3 targetPosition)
    {
        // Instruct the player to move to the clicked location
        // player.MoveTo(targetPosition);
        Debug.Log("Object Interaction:" + message);
        // TODO: Filter interations based on message or something.
        if (message == "movement_tile") {
            DISP = false;
            Player.HideMovementRange(Board);
            Player.Move(new Vector3(targetPosition.x, 0f, targetPosition.z), Board);
        }
    }


}
