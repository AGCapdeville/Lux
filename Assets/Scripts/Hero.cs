public class Hero : Entity
{
    public string HeroName { get; set; }
    public Vector3 position { get; set; }

    public Hero(float movement, int health, Vector3 position, Direction direction, string heroName, GameObject gamePiece)
        : base(movement, health, position, direction, gamePiece)
    {
        HeroName = heroName;
    }

    
    // Add hero-specific methods here
}