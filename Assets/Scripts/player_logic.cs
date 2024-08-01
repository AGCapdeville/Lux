using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_logic : MonoBehaviour
{   

    public bool moving;

    public Queue<Space> route; 

    private Space target; 

    [SerializeField] private float speed = 1; 

    // Start is called before the first frame update
    void Start()
    {
        moving = false;

        route = new Queue<Space>(); 

        target = null;  
    }

    // Update is called once per frame
    void Update()
    {   
        if (route.Count > 0 ) {   
            moving = true;
            
            target ??= route.Dequeue();
            
        }

        if (target != null && Vector3.Distance(transform.position, target.Position) > .05f) {
            transform.position = Vector3.MoveTowards(transform.position, target.Position, speed * Time.deltaTime);
        }
        else {
            target = null;
            moving = false;
        }
    }
}
