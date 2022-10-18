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
        if (_rb.velocity.x < 0.2 && _rb.velocity.y < 0.2 && Input.GetKey(KeyCode.S))
        {
            _cinemachineVirtualCamera.Follow = lookPos;
        }
        else
        {
            _cinemachineVirtualCamera.Follow = player;
        }
    }
}
