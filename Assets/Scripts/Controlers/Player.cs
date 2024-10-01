using UnityEngine;
using System.Collections.Generic;
using Vector3 = UnityEngine.Vector3;

public class Player
{
    public int ID { get; }
    public string Name { get; }
    public Dictionary<string, Hero> Party { get; }
    public string SelectedHero { set; get; }

    // Constructor to initialize event_id and event_name
    public Player(int id, string name)
    {
        ID = id;
        Name = name;
        Party = new Dictionary<string, Hero>();
        SelectedHero = "";
    }

    public void AddHeroToParty(Hero hero) {
        Party.Add(hero.HeroName, hero);
    }

    public bool SpaceIsInRange(Hero hero, Space targetSpace)
    {
        return Party[hero.HeroName].MovementRange.Contains(targetSpace);
    }

    public void MoveTo(Vector3 targetPosition, Queue<Space> path, Hero hero)
    {
        Party[hero.HeroName].Position = targetPosition;
        Party[hero.HeroName].HeroGameObject.GetComponent<UnitLogic>().route = path;  
    }

    public void UpdateMovementRange(HashSet<Space> newMoveRange, Hero hero)
    {
        Party[hero.HeroName].MovementRange = newMoveRange;
    }

}