using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeflectBullet : MonoBehaviour
{
    private bool inRange;
    public float bulletSpeed = 0.01f;
    private Vector3 shootDirection;
    private Vector3 target;
    private bool isBreakable;
    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
        }
        else if(other.CompareTag("Target") || other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            inRange = false;
        }
    }

    private void Deflect()
    {
        
        while (Input.GetKey(KeyCode.Mouse0))
        {
            Time.timeScale = 0.9f;
        }
        
        if (inRange && Input.GetKeyUp(KeyCode.Mouse0))
        {
            Time.timeScale = 1f;
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
