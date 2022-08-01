using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGate : MonoBehaviour
{

    public GameObject[] gates;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Key"))
        {
            for (int i = 0; i < gates.Length; i++)
            {
                Destroy(gates[i]);
                Destroy(gameObject);
            }
        }
    }
}
