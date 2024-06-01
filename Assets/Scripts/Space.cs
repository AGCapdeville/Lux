using System;
using UnityEngine;

public class Space
{
    // Properties
    public List<object> Content {get; set;} // everything that is on this location
    public GameObject Object {get;}

    public Dictionary<string, int> Position { get; set; }
    public int G { get; set; }
    public double H { get; set; }
    public double F { get; set; }
    public PathNode Prev { get; set; }
    public SpaceEnum Type { get; set; }
    public int Cost { get; set; }

    public enum SpaceEnum
    {
        Empty = 0,
        Block = 1,
        Player = 2,
        Enemy = 3
        // Add more mappings as needed
    }

    // Constructor
    public Space(Vector3 position, Vector3 scale, float weight = 1)
    {
        Position = position;
        GameObject space = Resources.Load<GameObject>("Space");
        Object = GameObject.Instantiate(space, Vector3.zero, Quaternion.identity);
        Object.transform.position = position;
        Object.transform.localScale = scale;
        
        G = 0;
        H = 0;
        F = 0;
        Cost = 1;
        Prev = null;
        Type = SpaceEnum.Empty;

    }

    /// <summary> Places an object on the board.</summary>
    /// <param name="obj">The object to place on the board.</param>    
    public void PlaceObject(Object obj) {
        try
        {
            Content.Add(obj);
        }
        catch (Exception e) 
        {
            Debug.Log(e.ToString());
        }
    }

}
