using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateBullet : MonoBehaviour
{
    private Transform _enemy;
    [SerializeField] private GameObject prefab;

    private void Start()
    {
        _enemy = GetComponent<Transform>();
        InvokeRepeating("Instantiate", 1.5f, 2.5f);
    }

    private void Instantiate()
    {
        var enemyPos = _enemy.position;
        var position = new Vector2(enemyPos.x, enemyPos.y);
        Instantiate(prefab, position, Quaternion.identity);
    }
}
