using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private GameObject weapon;
    [SerializeField] private InputManagerErick inputManager;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private int damage;
    private HealthManager _health;
    [SerializeField] private GameObject gold;

    public bool isAttacking;
    public float swingTimer;
    public float knockbackValue;
    public float knockbackSelf;

    private void Update()
    {
        Attack();
    }

    public void Attack()
    {
        if (inputManager.attackInput && !isAttacking)
        {
            isAttacking = true;   
            weapon.SetActive(true);
            StartCoroutine(AttackTimer());
        }
    }

    IEnumerator AttackTimer()
    {
        yield return new WaitForSeconds(swingTimer);
        isAttacking = false;
        weapon.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            var enemy = DealDamage(col);

            CheckHealth(enemy);
        }
    }

    private GameObject DealDamage(Collider2D col)
    {
        GameObject enemy = col.gameObject;
        _health = col.gameObject.GetComponent<HealthManager>();
        _health.UpdateHealth(damage);

        var knockBack = transform.position - col.transform.position;
        col.attachedRigidbody.AddForce(knockBack.normalized * -knockbackValue);
        rb.AddForce(knockBack.normalized * knockbackSelf);
        return enemy;
    }

    private void CheckHealth(GameObject enemy)
    {
        if (_health.Health <= 0)
        {
            var amount = Random.Range(1, 7);

            for (int i = 0; i < amount; i++)
            {
                var direction = Random.Range((float) -120, 120);
                var force = Random.Range(100, 300);
                var spawnedGold = Instantiate(gold, enemy.transform.position, quaternion.identity);
                spawnedGold.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction, force));
                Debug.Log(direction + " " + force + " " + amount);
            }

            Destroy(enemy);
        }
    }
}
