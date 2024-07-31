using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RhythmInput : MonoBehaviour
{
    private RhythmInput rhythmInput;

    void Start() 
    {
        rhythmInput = new RhythmInput();
    }

    private void OnUp() {
        Debug.Log("UP Arrow In Rythm..");
    }

}
