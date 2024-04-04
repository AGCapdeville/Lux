
using UnityEngine;
using System.Collections.Generic;

using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;

public class Player
{
    public int PlayerID { get; }
    public string Name { get; }
    public int Movement { get; }
    public GameObject Piece {get; set;}
    public Vector3 Direction {get; set;}
    private List<GameObject> MovementTiles;
    private HashSet<Space> MovementGridSpaces;

    // Constructor to initialize event_id and event_name
    public Player(int id, string name, int movement, Vector3 position, Board board)
    {
        PlayerID = id;
        Name = name;
        Movement = movement;
        Piece = new GameObject("player");
        Piece.transform.position = position;
        Direction = Vector3.forward;

        GameObject tri = Resources.Load<GameObject>("Triangle");
        GameObject triObj = GameObject.Instantiate(tri, Vector3.zero, Quaternion.identity);
        triObj.transform.parent = Piece.transform;

        UpdateMovementRange(board);
    }

    public void Move(Vector3 position, Board board) {
        Piece.transform.position = position;
        UpdateMovementRange(board);
    }

    // Takes in the current state of the game board, and retuns what spaces (tiles) 
    //   sprites should be rendered for move range of the player.
    public void DisplayMovementRange(Board board) {
        UpdateMovementRange(board);
        MovementTiles = new List<GameObject>();
        // Light up board with MovementGridSpaces, and create planes in those spaces...
        foreach (Space space in MovementGridSpaces)
        {
            GameObject movementTile = GameObject.Instantiate(board.MovementTile, Vector3.zero, Quaternion.identity);
            movementTile.transform.position = new Vector3(
                space.Object.transform.position.x,
                space.Object.transform.position.y + 1,
                space.Object.transform.position.z
            );

            MovementTiles.Add(movementTile);
            // DestroyAfterDelay(movementTile, 1f);
            // movementTile.transform.SetParent(space.transform); // Set the parent to make it a child of this GameObject
        }
    }
    public void HideMovementRange(Board board) {
        UpdateMovementRange(board);
        // Light up board with MovementGridSpaces, and create planes in those spaces...
        foreach (GameObject tile in MovementTiles)
        {
            GameObject.Destroy(tile);
        }
        MovementTiles = new List<GameObject>();
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

        HashSet<Space> MovementSpaces = new HashSet<Space>(); 
        bool found = false;

        foreach(Vector3 position in rangeSet){

            foreach(List<Space> row in board.Grid)
            {
                foreach(Space space in row){
                    if(space.Object.transform.position == position){
                         MovementSpaces.Add(space);
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
    public HashSet<Space> GetMovementRange(Board board) {

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

        HashSet<Space> MovementSpaces = new HashSet<Space>(); 
        bool found = false;

        foreach(Vector3 position in rangeSet){

            foreach(List<Space> row in board.Grid)
            {
                foreach(Space space in row){
                    if(space.Object.transform.position == position){
                        MovementSpaces.Add(space);
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

    private bool IsValidSpace(List<List<Space>> grid, Vector3 vectorToCheck ){

        List<Vector3> spaces = new List<Vector3>();

        foreach(List<Space> row in grid){
            foreach(Space space in row){
                spaces.Add(space.Object.transform.position);
            }
        }
        
        return spaces.Contains(vectorToCheck);
    }

}