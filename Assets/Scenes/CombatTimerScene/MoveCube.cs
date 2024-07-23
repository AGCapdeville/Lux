using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCube : MonoBehaviour
{
    public float speed = 1.0f;  // Speed of the cube's movement
    public float resetPosition = 5f;  // Position to reset to
    public float limitPosition = -5f;  // Position limit

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(resetPosition, 0, 0);    
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < limitPosition) {
            transform.position = new Vector3(resetPosition,0,0);
        } else {
            transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        }
    }
}
