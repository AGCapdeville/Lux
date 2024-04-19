using System;
using UnityEngine;

public class Space
{
    // Properties
    public GamePiece[] Contents {get; set;} // Content of space
    public GameObject Object {get;}
    public float Weight {get; set;}

    // Constructor
    public Space(Vector3 position, Vector3 scale, float weight = 1)
    {
        // GameObject movementTile = GameObject.Instantiate(board.MovementTile, Vector3.zero, Quaternion.identity);
        // Object = new GameObject();
        // Contents = piecesToBePlacedHere;

        GameObject space = Resources.Load<GameObject>("Space");
        Object = GameObject.Instantiate(space, Vector3.zero, Quaternion.identity);
        Object.transform.position = position;
        Object.transform.localScale = scale;
        Weight = weight;
    }
}
