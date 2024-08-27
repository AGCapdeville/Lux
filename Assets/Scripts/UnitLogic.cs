using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitLogic : MonoBehaviour
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

        if (target != null && Vector3.Distance(transform.position, target.Position) != 0) {

            Vector3 movementVector = Vector3.MoveTowards(transform.position, target.Position, speed * Time.deltaTime);    
            transform.position = movementVector;
            
            Vector3 lookVector = target.Position - transform.position;

            if ( lookVector != Vector3.zero || route.Count == 0 ) {
            
                transform.forward = target.Position - transform.position;

            }


            
            
        }
        else {
            target = null;
            moving = false;
        }
    }
}
