using UnityEngine;

public class Enemy : Entity
{
    public string EnemyType { get; set; }

    public Enemy(int id, float movement, int health, Vector3 position, Direction direction, string enemyType)
        : base(id, movement, health, position, direction)
    {
        EnemyType = enemyType;
    }

    // Add monster-specific methods here
}