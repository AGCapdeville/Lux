
using UnityEngine;
using System.Collections.Generic;

using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
using System.Runtime.ExceptionServices;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using System;
using UnityEngine.AI;

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

    public bool CanMoveTo(GameObject tragetSpace)
    {   
        foreach (Space i in Hero.MovementRange)
        {
            if (i.SpaceGameObject.transform.position == tragetSpace.transform.position)
            {
                return true;
            }
        }

        return false;
    }

    public void MoveTo(Vector3 targetPosition)
    {

        Hero.Position = targetPosition;
        Hero.HeroGameObject.transform.position = targetPosition;
    }

    public void UpdateMovementRange(HashSet<Space> newMoveRange)
    {
        Hero.MovementRange = newMoveRange;
    }

}