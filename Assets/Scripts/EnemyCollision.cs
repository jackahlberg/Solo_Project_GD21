using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{

    private Rigidbody2D _enemyRb;
    [SerializeField] private float knockBackValue;

    private void Start()
    {
        _enemyRb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            HealthManager healthManager = col.gameObject.GetComponent<HealthManager>();
            Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
            healthManager.DamageHealth(GetComponent<EnemyMelee>().damage);

            EnemyMelee slowdown = GetComponent<EnemyMelee>();
            StartCoroutine(slowdown.DamagedSlowDown());
            var knockBack = transform.position - col.transform.position;
            rb.AddForce(knockBack.normalized * -knockBackValue);
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            _enemyRb.isKinematic = true;
            _enemyRb.mass = 100f;
            _enemyRb.velocity = Vector2.zero;
            Debug.Log(_enemyRb.isKinematic);
        }
    }

    private void Update()
    {
        if (_enemyRb.isKinematic)
        {
            _enemyRb.isKinematic = false;
            _enemyRb.mass = 3f;
        }
    }
}
