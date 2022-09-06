using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Patrol : MonoBehaviour
{
    private bool _patrol;
    public float moveSpeed;
    [SerializeField] private Transform[] patrolPoints;
    private Transform _myScale;
    private Transform _player;
    private EnemyFollowPlayer _aggro;
    public int i;
    private Rigidbody2D _rb;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _aggro = GetComponent<EnemyFollowPlayer>();
        _myScale = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Vector2.Distance(_player.transform.position, transform.position) < 5)
        {
            enabled = false;
            _aggro.enabled = true;
        }
        
        Move();
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(moveSpeed * Time.fixedDeltaTime, _rb.velocity.y);
        Debug.Log(_rb.velocity);
    }

    private void Move()
    {
        //transform.position = Vector3.MoveTowards(transform.position, patrolPoints[i].transform.position, moveSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, patrolPoints[0].transform.position) < 0.5f && _myScale.localScale.x == 1)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            moveSpeed *= -1;
            i = 1;
        }
        if (Vector2.Distance(transform.position, patrolPoints[1].position) < 0.5f && _myScale.localScale.x == -1)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            moveSpeed *= -1;
            i = 0;
        }
    }
}
