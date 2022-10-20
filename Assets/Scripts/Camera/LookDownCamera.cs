using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor.Build.Content;

public class LookDownCamera : MonoBehaviour
{
    private CinemachineVirtualCamera _cinemachineVirtualCamera;
    [SerializeField] private Transform lookPos;
    [SerializeField] private Transform player;
    private Rigidbody2D _rb;
    private bool _lookingDown;
    
    void Start()
    {
        _rb = player.gameObject.GetComponent<Rigidbody2D>();
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        LookDown();
    }

    private void LookDown()
    {
        if (_rb.velocity.magnitude < 0.08f && Input.GetKey(KeyCode.S))
        {
            _cinemachineVirtualCamera.Follow = lookPos;
            _lookingDown = true;
        }
        else if(_lookingDown && _rb.velocity.magnitude > 3f || _lookingDown && !Input.GetKey(KeyCode.S))
        {
            _cinemachineVirtualCamera.Follow = player;
            _lookingDown = false;
        }
    }
}
