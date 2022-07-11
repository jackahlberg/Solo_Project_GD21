using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateBullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D player;
    [SerializeField] private GameObject prefab;

    private void Start()
    {
        InvokeRepeating("Instantiate", 3f, 2f);
    }

    private void Instantiate()
    {
        var playerPos = player.transform.position;
        var position = new Vector2(UnityEngine.Random.Range(playerPos.x + 10f, playerPos.x + 10), UnityEngine.Random.Range(playerPos.y + 10, playerPos.y -10));
        Instantiate(prefab, position, Quaternion.identity);
    }
}
