using UnityEngine;

public class Monster : Entity
{
    public string MonsterType { get; set; }

    public Monster(float movement, int health, Vector3 position, Direction direction, string monsterType, GameObject gamePiece)
        : base(movement, health, position, direction, gamePiece)
    {
        MonsterType = monsterType;
    }

    // Add monster-specific methods here
}