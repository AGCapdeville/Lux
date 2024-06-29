using System;
using UnityEngine;
using System.Collections.Generic;
using Scripts.Enums;

public class Space
{
    // Properties
    public GameObject SpaceGameObject {get; set;}
    public SpaceBehavior SpaceBehaviorScript {get; set;}
    public Vector3 Position { get; set; }
    public int G { get; set; }
    public double H { get; set; }
    public double F { get; set; }
    public Space Prev { get; set; }
    public SpaceEnum SpaceMarking { get; set; }
    public int Cost { get; set; }
    public Entity entity { get; set; }

    // Constructor
    public Space(GameObject board, Vector3 position, Vector3 scale, int cost = 1)
    {
        G = 0;
        H = 0;
        F = 0;
        Cost = cost;
        Prev = null;
        SpaceMarking = SpaceEnum.Empty;
        Position = position;

        SpaceGameObject = GameObject.Instantiate(
                            Resources.Load<GameObject>("Space"),
                            position,
                            Quaternion.identity
                        );

        // Adding Monobehavior Script & Keeping ref to script added
        SpaceBehaviorScript = SpaceGameObject.AddComponent<SpaceBehavior>();

        // Group all spaces under the board
        SpaceGameObject.transform.parent = board.transform;
    }


}
