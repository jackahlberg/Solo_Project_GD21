using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _player;
    [SerializeField] private GameObject _playerBullet;
    
    void Update()
    {
        var playerPos = _player.transform.position;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("Shoot");
            Instantiate(_playerBullet, playerPos, Quaternion.identity);
        }
    }
}
