using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackErick : MonoBehaviour
{
    [SerializeField] private GameObject weapon;
    [SerializeField] private InputManagerErick inputManager;
    [SerializeField] private Rigidbody2D rb;
    private HealthManager _health;
    [SerializeField] private int damage;

    public bool isAttacking;
    public float swingTimer;
    public float knockbackValue;
    public float knockbackSelf;

    //NEW
    private void Awake()
    {
        inputManager = gameObject.GetComponent<InputManagerErick>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        _health = gameObject.GetComponent<HealthManager>();
    }
    
    //===============================================================

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
            GameObject enemy = col.gameObject;
            _health = col.gameObject.GetComponent<HealthManager>();
            _health.DamageHealth(damage);

            var knockBack = transform.position - col.transform.position;
            col.attachedRigidbody.AddForce(knockBack.normalized * -knockbackValue);
            rb.AddForce(knockBack.normalized * knockbackSelf);
            
            if (_health.Health <= 0)
            {
                Destroy(enemy);
            }
        }
    }
}
