
using System.Collections;
using System.Collections.Generic;

using Vector3 = UnityEngine.Vector3;

public class Path
{
    public Queue p {set; get;}
    private Queue AllPaths {set; get;}
    private float StartingDistance = 0.0f;
    private HashSet<Space> ValidSpaces;
    private int MaxMovement;

    public Path(Space starting, Space ending, HashSet<Space> validSpaces, int maxMovement) {
        StartingDistance = Vector3.Distance(starting.Object.transform.position, ending.Object.transform.position);
        ValidSpaces = validSpaces;
        MaxMovement = maxMovement;
        // AllPaths = CalculateQueue(p, 0, starting);
        
    }

    // private Queue CalculateQueue(Queue s, int start, Space ) {
    //     Queue result = s;

    //     for (int d = start; d < MaxMovement; d++)
    //     {
            
    //         // build new q
    //         result.Enqueue(d);
            
    //         result = CalculateQueue(result, d);
    //     }
        
    //     return result;
    // }
    

    // [[]]


}
















// STARTING = <0,0,0>
// new Queue possiblePath = <0,0,0> (x4 for each direction)
//
// pass each path to a loop
// 


















