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
    public int i;

    void Update()
    {
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
