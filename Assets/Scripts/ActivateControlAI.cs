using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine;
public class ActivateControlAI : MonoBehaviour
{
    private PlayerController _playerController;
    private Transform _player;
    [SerializeField] private PlayerController controlledEnemy;
    [SerializeField] private Transform enemy;
    [SerializeField] private CinemachineVirtualCamera cam;

    private bool _inRange;
    private bool _isControlling;

    private void Start()
    {
        _playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        _player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        if (_inRange && Input.GetKeyDown(KeyCode.E) && !_isControlling)
        {
            _isControlling = true;
            _playerController.enabled = false;
            controlledEnemy.enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.E) && _isControlling)
        {
            _isControlling = false;
            _playerController.enabled = true;
            controlledEnemy.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            _inRange = true;
            cam.Follow = enemy;
            cam.LookAt = enemy;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            cam.Follow = _player;
            cam.LookAt = _player; 
            _inRange = false;
        }
    }
}
