
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class Player
{
    public int playerID { get; }
    public string heroName { get; }
    public int movement { get; }
    public GameObject playerPiece {get; set;}


    // Constructor to initialize event_id and event_name
    public Player(int id, string name, int initMovement, (int, int) initPosition)
    {
        playerID = id;
        heroName = name;
        movement = initMovement;
        playerPiece = new GameObject("player");
        playerPiece.transform.position = new Vector3(initPosition.Item1, 0f, initPosition.Item2);

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // Set the cube's position, rotation, and scale (optional)
        cube.transform.localScale = new Vector3(1f, 1f, 1f); // Example scale
        // Set the cube's parent to parent game object
        cube.transform.parent = playerPiece.transform;

    }

    public void movePlayer((int, int) newPosition) {
        playerPiece.transform.position = new Vector3(newPosition.Item1, 0f, newPosition.Item2);
    }

    // Takes in the current state of the game board, and retuns what spaces (tiles) 
    //   sprites should be rendered for move range of the player.
    public List<List<int>> displayMovementRange(List<List<GameObject>> board) {
        List<List<int>> boardHover = new List<List<int>>();

        return boardHover;
    }


    // Gets the spaces which the player can potentally move to and returns them.
    public HashSet<GameObject> getMovementRange(List<List<GameObject>> board){

        // THis needs to be passed from the creation of the board 
        // I put it here just for now to see if it is getting the correct movement range
        int spaceWidth = 5;
        int spaceLength = 5;

        HashSet<Vector3> rangeSet = new HashSet<Vector3>
        {
            //Starting Postion of the player
            playerPiece.transform.position
        };

        for(int i = 0; i < movement; i++){

            HashSet<Vector3> tempSet =  new HashSet<Vector3>();

            foreach(Vector3 pos in rangeSet){

                if(isValidSpace(board, new Vector3(pos.x + spaceWidth, pos.y, pos.z)))
                    tempSet.Add(new Vector3(pos.x + spaceWidth, pos.y, pos.z));
                
                if(isValidSpace(board, new Vector3(pos.x - spaceWidth, pos.y, pos.z)))
                    tempSet.Add(new Vector3(pos.x - spaceWidth, pos.y, pos.z));
                
                if(isValidSpace(board, new Vector3(pos.x, pos.y, pos.z  + spaceLength)))
                    tempSet.Add(new Vector3(pos.x, pos.y, pos.z  + spaceLength));
                
                if(isValidSpace(board, new Vector3(pos.x, pos.y, pos.z - spaceLength)))
                    tempSet.Add(new Vector3(pos.x, pos.y, pos.z - spaceLength));

            }

            rangeSet.UnionWith(tempSet);
        }

        HashSet<GameObject> MovementSpaces = new HashSet<GameObject>(); 
        bool found = false;

        foreach(Vector3 space in rangeSet){

            foreach(List<GameObject> row in board)
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

    private bool isValidSpace(List<List<GameObject>> board, Vector3 vectorToCheck ){

        List<Vector3> spaces = new List<Vector3>();

        foreach(List<GameObject> row in board){
            foreach(GameObject space in row){
                spaces.Add(space.transform.position);
            }
        }
        
        return spaces.Contains(vectorToCheck);
    }
}