using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RhythmGame : MonoBehaviour
{
    private RhythmActions _RhythmActions;

    void Awake() 
    {
        _RhythmActions = new RhythmActions();
    }

    private void OnEnable()
    {
        _RhythmActions.Enable();
        _RhythmActions.Arrows.Up.performed += OnUp;

    }

    private void OnDisable()
    {
        _RhythmActions.Disable();
        _RhythmActions.Arrows.Up.performed -= OnUp;
    }


    private void OnUp(InputAction.CallbackContext context) {
        Debug.Log("UP Arrow In Rythm..");
    }

}
