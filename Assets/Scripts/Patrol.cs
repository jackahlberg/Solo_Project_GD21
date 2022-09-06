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
    private Transform _player;
    private EnemyFollowPlayer _aggro;
    public int i;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _aggro = GetComponent<EnemyFollowPlayer>();
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

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, patrolPoints[i].transform.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, patrolPoints[0].transform.position) < 0.5f)
        {
            i = 1;
        }
        if (Vector2.Distance(transform.position, patrolPoints[1].position) < 0.5f)
        {
            i = 0;
        }
    }
}
