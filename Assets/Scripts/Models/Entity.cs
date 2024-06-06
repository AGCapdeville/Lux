using UnityEngine;

public class Entity
{
    public int ID { get; set; }
    public float Movement { get; set; }
    public int Health { get; set; }
    public Vector3 Position { get; set; }
    public Direction Direction { get; set; }
    public GameObject GamePiece;


    public Entity(int id, float movement, int health, Vector3 position, Direction direction, string gamePiece)
    {
        ID = id;
        Movement = movement;
        Health = health;
        Position = position;
        Direction = direction;

        GameObject gp = Resources.Load<GameObject>(gamePiece);
        GamePiece = GameObject.Instantiate(gp, Vector3.zero, Quaternion.identity);
        GamePiece.transform.position = position;
    }
    
    public void Move(Vector3 position) {
        GamePiece.transform.position = position;
    }

    // TODO: update to rotate towards direction.

    // public void rotateRight(){
    //     Vector3 rot = Piece.transform.localRotation.eulerAngles; 
    //     Piece.transform.rotation = Quaternion.Euler(rot.x, rot.y + 90, rot.z);
    // }
    // public void rotateLeft(){
    //     Vector3 rot = Piece.transform.localRotation.eulerAngles; 
    //     Piece.transform.rotation = Quaternion.Euler(rot.x, rot.y - 90, rot.z);
    // }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health < 0)
        {
            Health = 0;
        }
    }

    public bool IsAlive()
    {
        return Health > 0;
    }
}