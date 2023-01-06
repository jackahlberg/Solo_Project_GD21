using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawningManager : MonoBehaviour
{
    private GameObject _player;
    private Transform _spawnPoint;
    public StatsSO[] StatsSos;
    public int SOIndex;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        _spawnPoint = GameObject.Find("SpawnPoint").transform;
    }

    private void Start()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        _player.transform.position = _spawnPoint.position;
    }

}
