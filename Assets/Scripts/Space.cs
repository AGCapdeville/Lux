using System;
using UnityEngine;

public class Space
{
    // Properties
    public GamePiece[] Contents {get; set;} // Content of space
    public GameObject Object {get;}

    // Constructor
    public Space(Vector3 position, Vector3 scale)
    {
        // GameObject movementTile = GameObject.Instantiate(board.MovementTile, Vector3.zero, Quaternion.identity);
        GameObject space = Resources.Load<GameObject>("Space");
        Object = GameObject.Instantiate(space, Vector3.zero, Quaternion.identity);
        // Object = new GameObject();
        Object.transform.position = position;
        Object.transform.localScale = scale;
        // Contents = piecesToBePlacedHere;
    }
}
