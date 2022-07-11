using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class InstantiateBullet : MonoBehaviour
{

    [SerializeField] private GameObject prefab;

    private void Start()
    {
        InvokeRepeating(Instantiate(3f, 3f,));
    }

    private void Instantiate()
    {
        Instantiate(prefab, new Vector3( -3, 10, 0), Quaternion.identity);
    }
}
