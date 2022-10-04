using System;
using UnityEngine;
using Cinemachine;
public class ActivateControlAI : MonoBehaviour
{
    [SerializeField] private OfficialPlayerController playerController;
    [SerializeField] private Transform player;
    [SerializeField] private PlayerController controlledEnemy;
    [SerializeField] private Transform enemy;
    [SerializeField] private CinemachineVirtualCamera cam;
    private Rigidbody2D _enemyRb;
    private Rigidbody2D _playerRb;

    private bool _inRange;
    private bool _isControlling;

    private void Start()
    {
        
        _enemyRb = enemy.gameObject.GetComponent<Rigidbody2D>();
        _playerRb = player.gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_inRange && Input.GetKeyDown(KeyCode.E) && !_isControlling)
        {
            _isControlling = true;
            playerController.enabled = false;
            controlledEnemy.enabled = true;
            _playerRb.velocity = Vector3.zero;
        }
        else if (Input.GetKeyDown(KeyCode.E) && _isControlling)
        {
            _isControlling = false;
            playerController.enabled = true;
            controlledEnemy.enabled = false;
            _enemyRb.velocity = Vector3.zero;
            cam.Follow = player;
        }

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            _inRange = true;
            cam.Follow = enemy;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            cam.Follow = player;
            _inRange = false;
        }
    }
}
