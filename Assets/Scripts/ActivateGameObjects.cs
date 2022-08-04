using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateGameObjects : MonoBehaviour
{

    [SerializeField] private GameObject[] objects;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].SetActive(true);
            }
        }
    }
}
