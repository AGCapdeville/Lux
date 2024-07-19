using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArm : MonoBehaviour
{
    private int _min_range = -5;
    private int _max_range = 5;

    private Vector3 _current_pos;

    private bool headingRight = false;

    public float moveSpeed = 5f;  // Speed at which the GameObject moves


    private Renderer _renderer;
    private Material _white;
    private Material _arm;

    void Start () {
        _current_pos = transform.position;
        _renderer = GetComponent<Renderer>();
        _arm = Resources.Load<Material>("arm");
        _white = Resources.Load<Material>("white");

        // Start the coroutine that increases the value every second
        StartCoroutine(IncreaseValueCoroutine());
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if ( transform.position.x > -0.5f && transform.position.x < 0.5f) {
                if (transform.position.x > -0.8f && transform.position.x < 0.2f) {
                    Debug.Log("Critical HIT!");
                    StartCoroutine(FlareColor());
                } else {
                    Debug.Log("HIT!");
                }
            }
        }
    }

    private IEnumerator FlareColor() {
        _renderer.material = _white;
        yield return new WaitForSeconds(0.015f);
        _renderer.material = _arm;
    }

    // Coroutine to increase the value every second
    private IEnumerator IncreaseValueCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.0015f);

            if (headingRight) {
                if (_current_pos.x < _max_range) {
                    _current_pos.x += 0.01f;
                } else if (_current_pos.x >= _max_range) {
                    headingRight = false;
                }
            } else {
                if (_current_pos.x > _min_range) {
                    _current_pos.x -= 0.01f;
                } else if (_current_pos.x <= _min_range) {
                    headingRight = true;
                }
            }

            Vector3 newPosition = new Vector3(_current_pos.x, _current_pos.y, _current_pos.z);
            transform.position = newPosition;
            _current_pos = transform.position;
        }
    }

}
