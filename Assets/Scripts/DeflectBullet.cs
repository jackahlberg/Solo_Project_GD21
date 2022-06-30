using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeflectBullet : MonoBehaviour
{
    public Collider2D playerCol;
    private bool inRange;
    
    
    private void OnTriggerEnter2D(Collider2D playerCol)
    {
        inRange = true;
    }

    private void OnTriggerExit2D(Collider2D playerCol)
    {
        inRange = false;
    }

    private void AllowGrab()
    {

        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            Vector3 shootDirection = Input.mousePosition;
            shootDirection.z = 0f;
            shootDirection = Camera.main.ScreenToWorldPoint(shootDirection);
            Debug.Log("Grabbed Item");
            Debug.Log("ShotDirection: " + shootDirection.x + " " + shootDirection.y);

        }
    }

    void Update()
    {
        AllowGrab();
    }
}
