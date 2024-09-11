using System;
using UnityEngine;
using System.Collections.Generic;
using Scripts.Enums;

public enum SpaceEnum
{
    Empty = 0,
    Block = 1,
    Player = 2,
    Enemy = 3
    // Add more mappings as needed
}

public class Space
{
    // Properties
    public GameObject SpaceGameObject { get; set; }
    public SpaceBehavior SpaceBehaviorScript { get; set; }
    public Vector3 Position { get; set; }
    public int G { get; set; }
    public double H { get; set; }
    public double F { get; set; }
    public Space Prev { get; set; }
    public SpaceEnum SpaceMarking { get; set; }
    public int Cost { get; set; }
    public Entity entity { get; set; }

    public Dictionary<string, string> Walls { get; set; }

    public string Terrain { get; set; }

    // Constructor
    public Space(GameObject board, Vector3 position, Dictionary<string, string> walls, string terrain, int cost = 1)
    {
        G = 0;
        H = 0;
        F = 0;
        Cost = cost;
        Prev = null;
        SpaceMarking = SpaceEnum.Empty;
        Position = position;
        Walls = walls;
        Terrain = terrain;

        // update to use terrain for spaces:
        SpaceGameObject = GameObject.Instantiate(
                            Resources.Load<GameObject>("Space"),
                            position,
                            Quaternion.identity
                        );

        // Adding Monobehavior Script & Keeping ref to script added
        SpaceBehaviorScript = SpaceGameObject.AddComponent<SpaceBehavior>();

        // Group all spaces under the board
        SpaceGameObject.transform.parent = board.transform;

        // render walls!
    }


}
