using System;
using UnityEngine;
using System.Collections.Generic;
using Scripts.Enums;

public enum SpaceState
{
    Empty = 0,
    Occupied = 1,
    Block = 2
}

public class Space
{
    // Properties
    public GameObject SpaceGameObject { get; set; }
    public GameObject TerrainGameObject { get; set; }
    public SpaceBehavior SpaceBehaviorScript { get; set; }
    public Vector3 Position { get; set; }
    public int G { get; set; }
    public double H { get; set; }
    public double F { get; set; }
    public Space Prev { get; set; }
    public SpaceState State { get; set; }
    public int Cost { get; set; }
    public Unit unit { get; set; }

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

        Position = position;
        
        State = SpaceState.Empty;
        
        Walls = walls;
        
        Terrain = terrain;


        // update to use terrain for spaces:
        SpaceGameObject = GameObject.Instantiate(
                            Resources.Load<GameObject>("Prefabs/Space"),
                            position,
                            Quaternion.identity
                        );

        // Adding Monobehavior Script & Keeping ref to script added
        SpaceBehaviorScript = SpaceGameObject.AddComponent<SpaceBehavior>();

        // Group all spaces under the board
        SpaceGameObject.transform.parent = board.transform;

        if (terrain == "river") {
            TerrainGameObject = GameObject.Instantiate(
                                Resources.Load<GameObject>("Prefabs/WaterTerrain"),
                                position,
                                Quaternion.identity
                            );
        } else {
            TerrainGameObject = GameObject.Instantiate(
                                Resources.Load<GameObject>("Prefabs/Terrain"),
                                position,
                                Quaternion.identity
                            );
            Transform terrainObjectTransform = TerrainGameObject.transform.Find("TerrainObject");
            terrainObjectTransform.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/" + terrain);
        }
        // Add Terrain to Board
        TerrainGameObject.transform.parent = board.transform;

    }


}
