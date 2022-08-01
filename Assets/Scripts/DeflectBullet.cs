using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeflectBullet : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D bullet;
    [HideInInspector] public bool inRange;
    private MoveTowardsPlayer _movement;
    public float bulletSpeed = 0.01f;
    private Vector3 shootDirection;
    private Vector3 target;
    private bool isBreakable;
    public float setSlowDown;

    private void Start()
    {
        _movement = gameObject.GetComponent<MoveTowardsPlayer>();
        bullet = gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
        }
        else if(other.CompareTag("Bullet") || other.CompareTag("Key") || other.CompareTag("Target"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            inRange = false;
        }
    }

    private void Deflect()
    {
        if (inRange && Input.GetKey(KeyCode.Mouse0))
        {
            var arrow = FindObjectOfType<ArrowPointer>();
            arrow.sprite.enabled = true;
            Time.timeScale = setSlowDown;
            _movement.enabled = false;
        }
        
        if (inRange && Input.GetKeyUp(KeyCode.Mouse0))
        {
            var arrow = FindObjectOfType<ArrowPointer>();
            arrow.sprite.enabled = false;
            Time.timeScale = 1f;
            shootDirection = Input.mousePosition;
            shootDirection = Camera.main.ScreenToWorldPoint(shootDirection);
            target = (shootDirection - transform.position).normalized;
            gameObject.tag = "Key";
        }
    }

    void Update()
    {
        Deflect();
    }

    private void FixedUpdate()
    {
        transform.Translate(target.x * bulletSpeed, target.y * bulletSpeed, 0f, Space.World);
    }
}
