using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class CoinAttractor : MonoBehaviour
{
    public float speed;

    private void Awake()
    {
        StartCoroutine(CollisionTimer());
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            transform.position = 
                Vector3.MoveTowards(transform.position, other.gameObject.transform.position, speed * Time.deltaTime);
        }
    }

    IEnumerator CollisionTimer()
    {
        GetComponent<CircleCollider2D>().isTrigger = true;

        yield return new WaitForSeconds(0.2f);

        GetComponent<CircleCollider2D>().isTrigger = false;
    }
}
