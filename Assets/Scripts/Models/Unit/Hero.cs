using UnityEngine;
using System.Collections.Generic;

public class Hero : Unit
{
    public string HeroName { get; set; }
    public GameObject HeroGameObject { get; set; }
    public GameObject HeroPrefab { get; set; }
    public HashSet<Space> MovementRange { get; set; }
    public List<GameObject> MovementTiles { get; set; }

    public Hero(int id, float movement, float range,  int health, Vector3 position, Direction direction, string heroName, string prefabName)
        : base(id, movement, range, health, position, direction, "hero")
    {
        HeroName = heroName;
        GameObject HeroPrefab = Resources.Load<GameObject>("Prefabs/" + prefabName);
        HeroGameObject = GameObject.Instantiate(HeroPrefab, position, Quaternion.identity);
    }
}