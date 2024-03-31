
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
// using System;

public class Player
{
    public int PlayerID { get; }
    public string Name { get; }
    public int Movement { get; }
    public GameObject Piece {get; set;}

    private HashSet<GameObject> MovementGridSpaces;

    // Constructor to initialize event_id and event_name
    public Player(int id, string name, int movement, (int, int) position, Board board)
    {
        PlayerID = id;
        Name = name;
        Movement = movement;
        Piece = new GameObject("player");
        Piece.transform.position = new Vector3(position.Item1, 0f, position.Item2);

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // Set the cube's position, rotation, and scale (optional)
        cube.transform.localScale = new Vector3(1f, 1f, 1f); // Example scale
        // Set the cube's parent to parent game object
        cube.transform.parent = Piece.transform;

        UpdateMovementRange(board);

    }

    public void Move((int, int) position, Board board) {
        Piece.transform.position = new Vector3(position.Item1, 0f, position.Item2);
        UpdateMovementRange(board);
    }

    // Takes in the current state of the game board, and retuns what spaces (tiles) 
    //   sprites should be rendered for move range of the player.
    public void DisplayMovementRange(Board board) {
        UpdateMovementRange(board);

        // Light up board with MovementGridSpaces, and create planes in those spaces...
        foreach (GameObject space in MovementGridSpaces)
        {

            GameObject movementTile = GameObject.Instantiate(board.MovementTile, Vector3.zero, Quaternion.identity);
            movementTile.transform.position = space.transform.position;
            DestroyAfterDelay(movementTile, 1f);
            // movementTile.transform.SetParent(space.transform); // Set the parent to make it a child of this GameObject
        }
    }

    public void MoveToRandomBoardSpace(Board board) {
        int location = GetRandomNumber(0, MovementGridSpaces.Count);
        int index = 0;
        foreach (GameObject space in MovementGridSpaces) {
            if (index++ == location) {
                Piece.transform.position = space.transform.position;
                UpdateMovementRange(board);
            }
        }
    }

    static int GetRandomNumber(int minValue, int maxValue)
    {
        // Create a Random object
        System.Random random = new System.Random();
        // Generate a random number within the range
        int randomNumber = random.Next(minValue, maxValue + 1);
        return randomNumber;
    }

    private void DestroyAfterDelay(GameObject gameObjectToDestroy, float delay)
    {
        // Destroy the gameObject after the specified delay
        GameObject.Destroy(gameObjectToDestroy, delay);
    }


    public void UpdateMovementRange(Board board) {

        HashSet<Vector3> rangeSet = new HashSet<Vector3>
        {
            //Starting Postion of the player
            Piece.transform.position
        };

        for(int i = 0; i < Movement; i++){

            HashSet<Vector3> tempSet =  new HashSet<Vector3>();

            foreach(Vector3 pos in rangeSet){

                if(IsValidSpace(board.Grid, new Vector3(pos.x + board.SpaceWidth, pos.y, pos.z)))
                    tempSet.Add(new Vector3(pos.x + board.SpaceWidth, pos.y, pos.z));
                
                if(IsValidSpace(board.Grid, new Vector3(pos.x - board.SpaceWidth, pos.y, pos.z)))
                    tempSet.Add(new Vector3(pos.x - board.SpaceWidth, pos.y, pos.z));
                
                if(IsValidSpace(board.Grid, new Vector3(pos.x, pos.y, pos.z  + board.SpaceHeight)))
                    tempSet.Add(new Vector3(pos.x, pos.y, pos.z  + board.SpaceHeight));
                
                if(IsValidSpace(board.Grid, new Vector3(pos.x, pos.y, pos.z - board.SpaceHeight)))
                    tempSet.Add(new Vector3(pos.x, pos.y, pos.z - board.SpaceHeight));

            }

            rangeSet.UnionWith(tempSet);
        }

        HashSet<GameObject> MovementSpaces = new HashSet<GameObject>(); 
        bool found = false;

        foreach(Vector3 space in rangeSet){

            foreach(List<GameObject> row in board.Grid)
            {
                foreach(GameObject boardSpace in row){
                    if(boardSpace.transform.position == space){
                         MovementSpaces.Add(boardSpace);
                         found = true;
                         break;
                    }
                }
                if(found){
                    found = false;
                    break;
                }
            }
        }
        MovementGridSpaces = MovementSpaces;
    }


    // Gets the spaces which the player can potentally move to and returns them.
    public HashSet<GameObject> GetMovementRange(Board board) {

        HashSet<Vector3> rangeSet = new HashSet<Vector3>
        {
            //Starting Postion of the player
            Piece.transform.position
        };

        for(int i = 0; i < Movement; i++){

            HashSet<Vector3> tempSet =  new HashSet<Vector3>();

            foreach(Vector3 pos in rangeSet){

                if(IsValidSpace(board.Grid, new Vector3(pos.x + board.SpaceWidth, pos.y, pos.z)))
                    tempSet.Add(new Vector3(pos.x + board.SpaceWidth, pos.y, pos.z));
                
                if(IsValidSpace(board.Grid, new Vector3(pos.x - board.SpaceWidth, pos.y, pos.z)))
                    tempSet.Add(new Vector3(pos.x - board.SpaceWidth, pos.y, pos.z));
                
                if(IsValidSpace(board.Grid, new Vector3(pos.x, pos.y, pos.z  + board.SpaceHeight)))
                    tempSet.Add(new Vector3(pos.x, pos.y, pos.z  + board.SpaceHeight));
                
                if(IsValidSpace(board.Grid, new Vector3(pos.x, pos.y, pos.z - board.SpaceHeight)))
                    tempSet.Add(new Vector3(pos.x, pos.y, pos.z - board.SpaceHeight));

            }

            rangeSet.UnionWith(tempSet);
        }

        HashSet<GameObject> MovementSpaces = new HashSet<GameObject>(); 
        bool found = false;

        foreach(Vector3 space in rangeSet){

            foreach(List<GameObject> row in board.Grid)
            {
                foreach(GameObject boardSpace in row){
                    if(boardSpace.transform.position == space){
                         MovementSpaces.Add(boardSpace);
                         found = true;
                         break;
                    }
                }
                if(found){
                    found = false;
                    break;
                }
            }
        }
        
        return MovementSpaces;
    }

    private bool IsValidSpace(List<List<GameObject>> grid, Vector3 vectorToCheck ){

        List<Vector3> spaces = new List<Vector3>();

        foreach(List<GameObject> row in grid){
            foreach(GameObject space in row){
                spaces.Add(space.transform.position);
            }
        }
        
        return spaces.Contains(vectorToCheck);
    }
}