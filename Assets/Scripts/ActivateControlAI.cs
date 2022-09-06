using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class ActivateControlAI : MonoBehaviour
{
    private PlayerController _player;
    [SerializeField] private ControlledMovement controlledEnemy;

    private bool _inRange;
    private bool _isControlling;

    private void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (_inRange && Input.GetKeyDown(KeyCode.E) && !_isControlling)
        {
            _isControlling = true;
            _player.enabled = false;
            controlledEnemy.enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.E) && _isControlling)
        {
            Debug.Log("Triggers");
            _isControlling = false;
            _player.enabled = true;
            controlledEnemy.enabled = false;
        }
    }

    private void FixedUpdate()
    {

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("In range");
            _inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _inRange = false;
        }
    }
}
