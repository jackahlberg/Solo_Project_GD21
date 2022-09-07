using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{

    [SerializeField] private GameObject weapon;
    [SerializeField] private EnemyFollowPlayer follow;
    [SerializeField] private SpriteRenderer color;
    public float swingTimer = 0.5f;
    public float anticipationTime = 1;

    public void Attack()
    {
        StartCoroutine(SwingTimer());
    }

    IEnumerator SwingTimer()
    {
        color.color= Color.black;
        yield return new WaitForSeconds(anticipationTime);
        color.color = Color.white;
        weapon.SetActive(true);
        yield return new WaitForSeconds(swingTimer);
        weapon.SetActive(false);
        follow.enabled = true;
        enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            HealthContainer playerHealth = col.gameObject.GetComponent<HealthContainer>();
            playerHealth.health--;

            Time.timeScale = 0.5f;

            StartCoroutine(DamagedSlowDown());
        }
    }

    IEnumerator DamagedSlowDown()
    {
        yield return new WaitForSeconds(0.25f);

        Time.timeScale = 1;
    }
}
