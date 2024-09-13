using System.Collections.Generic;
using UnityEngine;

public class Hero : Unit
{
    public string HeroName { get; set; }
    public GameObject HeroGameObject { get; set; }
    public GameObject HeroPrefab { get; set; }
    public HashSet<Space> MovementRange { get; set; }
    public List<GameObject> MovementTiles { get; set; }

    public Hero(int id, float movement, int health, Vector3 position, Direction direction, string heroName, string prefabName)
        : base(id, movement, health, position, direction, "hero")
    {
        HeroName = heroName;
        GameObject HeroPrefab = Resources.Load<GameObject>("Prefabs/" + prefabName);
        HeroGameObject = GameObject.Instantiate(HeroPrefab, Vector3.zero, Quaternion.identity);
    }
}