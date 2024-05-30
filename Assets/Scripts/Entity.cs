
public class Entity
{
    public float Movement { get; set; }
    public int Health { get; set; }
    public Vector3 Position { get; set; }
    public Direction Direction { get; set; }
    private GameObject GamePiece;


    public Entity(float movement, int health, Vector3 position, Direction direction, GameObject gamePiece)
    {
        Movement = movement;
        Health = health;
        Position = position;
        Direction = direction;
        this.GamePiece = gamePiece; // prefab
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