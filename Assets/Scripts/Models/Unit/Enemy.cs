using UnityEngine;
using System.Collections.Generic;

public class Enemy : Unit
{
    public string EnemyName { get; set; }
    public GameObject EnemyGameObject { get; set; }
    public GameObject EnemyPrefab { get; set; }
    public HashSet<Space> MovementRange { get; set; }
    public List<GameObject> MovementTiles { get; set; }
    public Enemy(int id, float movement, int health, Vector3 position, Direction direction, string enemyName, string prefabName)
        : base(id, movement, health, position, direction, "enemy")
    {
        EnemyName = enemyName;
        GameObject EnemyPrefab = Resources.Load<GameObject>("Prefabs/" + prefabName);
        EnemyGameObject = GameObject.Instantiate(EnemyPrefab, position, Quaternion.identity);
    }
}