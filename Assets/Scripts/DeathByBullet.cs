using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class DeathByBullet : MonoBehaviour
{
    private GameObject _me;
    private GameObject _bullet;

    private DeathOnCollision _checkpoint;

    private void Start()
    {
        _me = gameObject;
        _checkpoint = GetComponent<DeathOnCollision>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (_me.CompareTag("Player") && col.CompareTag("Bullet") || _me.CompareTag("Player") && col.CompareTag("Key"))
        {
            _bullet = col.gameObject;
            
            StartCoroutine(DeflectCheck());
        }
        else if (_me.CompareTag("Enemy") && col.CompareTag("Bullet") || _me.CompareTag("Enemy") && col.CompareTag("Key"))
        {
            Destroy(gameObject);
        }
    }

    IEnumerator DeflectCheck()
    {
        yield return new WaitForSeconds(0.25f);
        
        if (Vector2.Distance(_bullet.transform.position, _me.transform.position) < 1.5f)
        {
            transform.position = _checkpoint.respawnPoint;
            Destroy(_bullet);
        }
    }
}