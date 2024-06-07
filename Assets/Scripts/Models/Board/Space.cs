using System;
using UnityEngine;
using System.Collections.Generic;
using Scripts.Enums;

public class Space
{
    // Properties
    public List<object> Content {get; set;} // everything that is on this location

    public GameObject SpaceGameObject {get; set;}
    private AudioManager audioManager;



    public Vector3 Position { get; set; }
    public int G { get; set; }
    public double H { get; set; }
    public double F { get; set; }
    public Space Prev { get; set; }
    public SpaceEnum SpaceMarking { get; set; }
    public int Cost { get; set; }

    // Constructor
    public Space(Vector3 position, Vector3 scale, int cost = 1)
    {
        Position = position;
        
        // Handles all the Start(), Update(), etc logic...
        SpaceGameObject = GameObject.Instantiate(
                            Resources.Load<GameObject>("Space"),
                            Vector3.zero,
                            Quaternion.identity
                        );
        SpaceGameObject.transform.position = position;
        SpaceGameObject.transform.localScale = scale;
        
        G = 0;
        H = 0;
        F = 0;
        Cost = cost;
        Prev = null;
        SpaceMarking = SpaceEnum.Empty;
    }

    /// <summary> Places an object on the board.</summary>
    /// <param name="obj">The object to place on the board.</param>    
    public void PlaceObject(object obj) {
        // TODO: (should still do this...)
        // This should spawn the object onto the space as well.. 
        // Not just store it into memory etc

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
