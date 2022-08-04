using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeflectBullet : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D bullet;
    [HideInInspector] public bool inRange;
    private CircleCollider2D _col;
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
        _col = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
        }
        else if(other.CompareTag("Bullet") || other.CompareTag("Key") || other.CompareTag("Target") || other.gameObject.layer == 8 || other.CompareTag("Enemy"))
        {
            StartCoroutine(WaitToDestroy());
        }

    }

    IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(0.03f);
        Destroy(gameObject);
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
        if (inRange && Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                var arrow = FindObjectOfType<ArrowPointer>();
                arrow.sprite.enabled = true;
                Time.timeScale = setSlowDown;
                _movement.enabled = false;
            }
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
            _col.radius = 0.63f;
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
