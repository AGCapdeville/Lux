using UnityEngine;

public class Hero : Entity
{
    public string HeroName { get; set; }
    public GameObject HeroGameObject { get; set; }
    public GameObject HeroPrefab { get; set; }

    public Hero(int id, float movement, int health, Vector3 position, Direction direction, string heroName, string prefabName)
        : base(id, movement, health, position, direction)
    {
        HeroName = heroName;
        // player...
        GameObject HeroPrefab = Resources.Load<GameObject>(prefabName);
        GameObject HeroGameObject = GameObject.Instantiate(HeroPrefab, Vector3.zero, Quaternion.identity);
    }
    
}