using UnityEngine;

public class Hero : Entity
{
    public string HeroName { get; set; }
    public Vector3 position { get; set; }

    public Hero(int id, float movement, int health, Vector3 position, Direction direction, string heroName, GameObject gamePiece)
        : base(id, movement, health, position, direction, gamePiece)
    {
        HeroName = heroName;
    }

    
    // Add hero-specific methods here
}