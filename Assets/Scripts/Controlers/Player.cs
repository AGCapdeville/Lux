
using UnityEngine;
using System.Collections.Generic;

using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
using System.Runtime.ExceptionServices;

public class Player
{
    public int ID { get; }
    public string Name { get; }
    public Hero Hero { set; get; }

    // Constructor to initialize event_id and event_name
    public Player(int id, string name, Hero hero)
    {
        ID = id;
        Name = name;        
        Hero = hero;
    }

}