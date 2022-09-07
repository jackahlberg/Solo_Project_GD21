using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class ActivatePlatform : MonoBehaviour
{
    [SerializeField] private Transform platform;
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform endPos;
    private bool _onPressurePlate = false;
    
    public float openSpeed = 5f;
    public float closingSpeed = 5f;

    private void Update()
    {
        if (_onPressurePlate)
        {
            EndPos();
        }
        
        if (!_onPressurePlate)
        {
            StartPos();
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        _onPressurePlate = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _onPressurePlate = false;
    }

    private void EndPos()
    {
        platform.transform.position = Vector3.MoveTowards(platform.transform.position, endPos.transform.position,
            openSpeed * Time.deltaTime);
    }
    
    private void StartPos()
    {
        platform.transform.position = Vector3.MoveTowards(platform.transform.position, startPos.transform.position, 
            closingSpeed * Time.deltaTime);
    }
}
