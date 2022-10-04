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
        }
    }

    private GameObject DealDamage(Collider2D col)
    {
        GameObject enemy = col.gameObject;
        _health = col.gameObject.GetComponent<HealthManager>();
        _health.DamageHealth(damage);

        var knockBack = transform.position - col.transform.position;
        col.attachedRigidbody.AddForce(knockBack.normalized * -knockbackValue);
        rb.AddForce(knockBack.normalized * knockbackSelf);
        return enemy;
    }
}
