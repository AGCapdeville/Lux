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
    void Start()
    {
        int rows = 5;
        int columns = 5;
        int spaceWidth = 10; // Looks like 10 equates to 1 unit in plane
        int spaceHeight = 10;

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

            var blockedLocations = new List<(int, int)> { (0, 3), (1, 2) };
            int distance = 10;
            var mapData = GenerateMap(blockedLocations, distance);
            
            Node playerNode = new Node(0,0);
            playerNode.Type = SpaceEnum.Player;
            Node endNode = new Node(0,40);

            Pathing playerPathing = new Pathing(mapData, distance);
            playerPathing.ClosedList[(playerNode.Position["x"], playerNode.Position["y"])] = playerNode;

            List<Node> path = playerPathing.FindPath(playerNode, endNode);

            foreach (var n in path)
            {
                Debug.Log($"({n.Position["x"]}, {n.Position["y"]})");
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
