using UnityEngine;

public class Unit
{
    public int ID { get; set; }
    public float Movement { get; set; }
    public int Health { get; set; }
    public Vector3 Position { get; set; }
    public Direction Direction { get; set; }
    public string Type { get; set; }


    public Unit(int id, float movement, int health, Vector3 position, Direction direction, string type)
    {
        ID = id;
        Movement = movement;
        Health = health;
        Position = position;
        Direction = direction;
        Type = type;
    }

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