using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeflectBullet : MonoBehaviour
{
    public Collider2D playerCol;
    private bool inRange;
    public float bulletSpeed = 0.01f;
    private Vector3 shootDirection;
    private Vector3 target;
    private bool isBreakable;
    
    
    private void OnTriggerEnter2D(Collider2D playerCol)
    {
        inRange = true;
    }

    private void OnTriggerExit2D(Collider2D playerCol)
    {
        inRange = false;
    }

    private void OntriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player") && !col.CompareTag("Ground"))
        {
            
        }
    }

    private void Deflect()
    {
        if (inRange && Input.GetKey(KeyCode.Mouse0))
        {
            shootDirection = Input.mousePosition;
            shootDirection = Camera.main.ScreenToWorldPoint(shootDirection);
            target = (shootDirection - transform.position).normalized;
            isBreakable = true;
        }
    }

    void Update()
    {
        Deflect();
        transform.Translate(target.x * bulletSpeed, target.y * bulletSpeed, 0f, Space.World);
    }
}
