using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningManager : MonoBehaviour
{
    private GameObject _player;
    private Transform _spawnPoint;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        _spawnPoint = GameObject.Find("SpawnPoint").transform;
    }

    private void Start()
    {
        _player.transform.position = _spawnPoint.position;
        Destroy(this);
    }

}
